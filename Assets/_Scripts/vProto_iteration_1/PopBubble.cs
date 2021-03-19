using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bubble))]
public class PopBubble : MonoBehaviour
{
    [HideInInspector]
    public bool processed = false;
    protected Bubble compoBubble;

    [Header("Puntos que da la burbuja al explotar")]
    public int pointsOnPop = 100;

    //// Start is called before the first frame update
    protected void Start()
    {
        this.compoBubble = GetComponent<Bubble>();
    }


    /// <summary>
    /// resetear el estado de procesado para reutilizar el recurso
    /// mostrar particulas
    /// mandar al pool
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator Pop()
    {
        processed = false; // false?
        PointsManager.instance.InstantiatePointAddEffect(transform.position);
        PointsManager.instance.AddToTotal(pointsOnPop);
        yield return null;
        Debug.Log("poping " + gameObject.name);

        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);

    }

    /// <summary>
    /// comienza la operacion de busqueda (usar 1 vez desde el primer impacto)
    /// </summary>
    /// <param name="matchType"></param>
    /// <param name="matchByType"></param>
    /// <returns></returns>
    public IEnumerator StartNeighborScan(BubbleType matchType, bool matchByType = true)
    {
        yield return null;

        //Debug.Log("Starting scan at " + this.compoBubble.name);
        TileGrid.instance.cluster = new List<Bubble>();

        processed = false;

        yield return StartCoroutine(SearchAnidado(matchType, matchByType));

        yield return null;

        TileGrid.instance.SetCluster(this.compoBubble, !matchByType);
        // si no matcheo por tipo, fuerzo la operacion de limpiar el cluster del mapa

    }

    public virtual IEnumerator SearchAnidado(BubbleType matchType, bool matchByType = true)
    {
        // me fijo si :
        // - el tipo es igual
        // - es un especial que no matchea por color
        // - ya procese este vecino
        if ((matchType.Equals(compoBubble.type) && matchByType) && !processed)
        {
            processed = true;
            TileGrid.instance.cluster.Add(this.compoBubble);

            BubbleNeighbor myNeighbors = new BubbleNeighbor();
            foreach (var neighbor in myNeighbors.GetWithoutDiagonals()) //GetTileOffsetsBasedOnParity(compoBubble.rowRaw % 2))
            {
                // lista de vectores de offset para agregar a la posicion de
                // composite bubble.

                // reviso estar en rango y que tenga sentido hacer la comparacion
                if (compoBubble.rowRaw - (int)neighbor.y >= 0 &&
                    compoBubble.rowRaw - (int)neighbor.y < TileGrid.instance.grid.GetLength(1))
                {
                    if (compoBubble.colRaw + (int)neighbor.x >= 0 &&
                    compoBubble.colRaw + (int)neighbor.x < TileGrid.instance.grid.GetLength(0))
                    {
                        // estando en la grilla, reviso el tipo
                        var target = TileGrid.instance.grid[compoBubble.colRaw + (int)neighbor.x, compoBubble.rowRaw - (int)neighbor.y];
                        // si es valido y es una burbuja, hago otro search anidado a sus vecinos

                        if (target != null && target.gameObject.activeInHierarchy)
                        {
                            RaycastHit2D recheck = Physics2D.Raycast(transform.position, target.transform.position - transform.position);
                            Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red, 2f);
                            Debug.Log("Drawing ray recheck");
                            //yield return new WaitForSeconds(2f);


                            if (recheck.collider != null)
                            {
                                if (matchType.Equals(this.compoBubble.type) && matchType.Equals(recheck.collider.GetComponent<Bubble>().type))
                                {
                                    yield return StartCoroutine(target.GetComponent<PopBubble>().SearchAnidado(matchType, matchByType));
                                }
                            }

                        }

                    }
                }
            }
        }
        else if (!matchByType && !processed)
        {
            switch (compoBubble.type.type)
            {
                /// todos estos casos deben estar definidos en la lista de burbujas
                /// especiales (guardada en BubbleResources). 
                case "line":
                    processed = true;
                    for (int col = 0; col < TileGrid.instance.grid.GetLength(0); col++)
                    {
                        if(compoBubble.rowRaw >= 1 && compoBubble.rowRaw < TileGrid.instance.grid.GetLength(1))
                        {
                            TileGrid.instance.cluster.Add(TileGrid.instance.grid[col, this.compoBubble.rowRaw]);
                            TileGrid.instance.cluster.Add(TileGrid.instance.grid[col, compoBubble.rowRaw - 1]);
                            Debug.Log("Doble fila deleteada");
                        }
                        else if (compoBubble.rowRaw == 0)
                        {
                            TileGrid.instance.cluster.Add(TileGrid.instance.grid[col, this.compoBubble.rowRaw]);
                            Debug.Log("Fila techo deleteada");
                        }

                    }
                    break;
            }
        }
    }

}
