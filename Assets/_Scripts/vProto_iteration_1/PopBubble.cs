using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bubble))]
public class PopBubble : MonoBehaviour
{
    [HideInInspector]
    public bool processed = false;
    Bubble compoBubble;

    [Header("Puntos que da la burbuja al explotar")]
    public int pointsOnPop = 100;

    //// Start is called before the first frame update
    void Start()
    {
        this.compoBubble = GetComponent<Bubble>();
    }
    private void OnDisable()
    {
        if (!processed)
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// resetear el estado de procesado para reutilizar el recurso
    /// mostrar particulas
    /// mandar al pool
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pop()
    {
        processed = false;
        PointsManager.instance.InstantiatePointAddEffect(transform.position);
        PointsManager.instance.AddToTotal(pointsOnPop);
        //yield return null;
        yield return null;
        gameObject.SetActive(false);
        
    }


    public IEnumerator StartNeighborScan(BubbleType matchType, bool matchByType = true)
    {
        Debug.Log("Starting scan at " + this.compoBubble.name);
        TileGrid.instance.cluster = new List<Bubble>();

        processed = false;
        TileGrid.instance.cluster.Add(this.compoBubble);

        yield return StartCoroutine(SearchAnidado(matchType, matchByType));

        yield return null;
        Debug.Log(TileGrid.instance.cluster.Count);

        if (TileGrid.instance.cluster.Count >= 3)
        {

            //exploto la burbuja y hago los cambios visuales necesarios.
            foreach (var bb in TileGrid.instance.cluster)
                //for (var i = TileGrid.instance.cluster.Count - 1; i >= 0; i--)
                //{
                //    //if()
                //TileGrid.instance.cluster[i]
                if (bb.gameObject.activeSelf)
                    bb.GetComponent<PopBubble>().StartCoroutine(Pop());
            //}
        }
        else
        {

            //reseteo los estados de procesado para poder hacer combo de vuelta cuando haya suficientes.
            foreach (var bb in TileGrid.instance.cluster)
            {
                bb.GetComponent<PopBubble>().processed = false;
            }
        }

    }

    public IEnumerator SearchAnidado(BubbleType matchType, bool matchByType = true)
    {
        // me fijo si :
        // - el tipo es igual
        // - es un especial que no matchea por color
        // - ya procese este vecino
        if ((matchType.Equals(compoBubble.type) || !matchByType) && !processed)
        {
            processed = true;
            TileGrid.instance.cluster.Add(this.compoBubble);

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
                        // su es valido y es una burbuja, hago otro search anidado a sus vecinos
                        if (target != null)
                            yield return StartCoroutine(target.GetComponent<PopBubble>().SearchAnidado(matchType, matchByType));

                        //// espero que termine el frame para resolver
                        //yield return null;
                        //Debug.Log(TileGrid.instance.cluster.Count + " en cluster");

                    }
                }
            }
        }






    }

}
