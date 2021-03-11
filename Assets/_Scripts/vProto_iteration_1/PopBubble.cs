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

        Debug.Log("Starting scan at " + this.compoBubble.name);
        TileGrid.instance.cluster = new List<Bubble>();

        processed = false;

        yield return StartCoroutine(SearchAnidado(matchType, matchByType));

        Debug.Log(TileGrid.instance.cluster.Count);

        yield return null;

        TileGrid.instance.SetCluster();

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
            Debug.Log(this.compoBubble + " added");

            BubbleNeighbor myNeighbors = new BubbleNeighbor();
            foreach (var neighbor in myNeighbors.GetTileOffsetsBasedOnParity(compoBubble.rowRaw % 2))
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
                        if (target != null)
                            yield return StartCoroutine(target.GetComponent<PopBubble>().SearchAnidado(matchType, matchByType));


                    }
                }
            }
        }
        else if(!matchByType && !processed)
        {
            switch (compoBubble.type.type)
            {
                /// todos estos casos deben estar definidos en la lista de burbujas
                /// especiales que esta guardada en BubbleResources. 
                case "line":
                    processed = true;
                    for (int col = 0; col < TileGrid.instance.grid.GetLength(0); col++)
                    {
                        TileGrid.instance.cluster.Add(TileGrid.instance.grid[col, this.compoBubble.rowRaw]);
                    }
                    break;
            }
        }
    }

}
