using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGrid : MonoBehaviour
{
    public static TileGrid instance { get; private set; }

    [SerializeField]
    int rows = 20;
    [SerializeField]
    int cols = 11;

    public float tileSize = 0.5f;

    public BubbleNeighbor neighborOffsetArray;

    public Bubble[,] grid;

    public List<Bubble> cluster;
    public List<Bubble> floatingclusters;

    public UnityEvent<int, int, int> onRemoveCluster;
    public UnityEvent onResetProcessed;


    private void Awake()
    {
        instance = this;

        Debug.Log("Generando mapa...");
        GenerateGrid();
        neighborOffsetArray = new BubbleNeighbor();
        onRemoveCluster = new UnityEvent<int, int, int>();
        onResetProcessed = new UnityEvent();

        cluster = new List<Bubble>();
        floatingclusters = new List<Bubble>();

    }

    // Start is called before the first frame update
    void Start()
    {
        AdmobComponentsManager.instance.onSendToBackAds.Invoke();
    }

    private void GenerateGrid()
    {
        int rowsForPlayer = 10;
        //rows = rows; // Posible problema en reinicio de mapa
        instance.grid = new Bubble[cols, rows + rowsForPlayer];


        for (int row = 0; row < rows - rowsForPlayer; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject refTile = (GameObject)ObjectPooler.instance.SpawnFromPool("bubble");
                refTile.name = "bubble_" + col.ToString() + row.ToString();
                var bubble = refTile.GetComponent<Bubble>();

                bubble.OnObjectSpawn(row, col, tileSize);
                instance.grid[col, row] = bubble;
                instance.grid[col, row].gameObject.SetActive(true);
            }
        }

        float gridWidth = instance.grid.GetLength(0) * tileSize;
        float gridHeight = instance.grid.GetLength(1) * tileSize;

        transform.position = new Vector3(-2.5f, gridHeight * tileSize * 2.75f - rowsForPlayer);
    }

    public void SetCluster(Bubble baseSearch, bool force = false)
    {
        StartCoroutine(ClusterPopOrReset(baseSearch));
    }

    IEnumerator ClusterPopOrReset(Bubble baseSearch, bool force = false)
    {
        int maxRowHitIdx = baseSearch.rowRaw;

        yield return null;

        if (instance.cluster.Count >= 3 && !force || force)
        {
            //exploto la burbuja y hago los cambios visuales necesarios.
            foreach (var bb in instance.cluster.FindAll(b => b != null))
            {
                if (bb.gameObject.activeInHierarchy)
                {
                    var target = bb.GetComponent<PopBubble>();

                    if (target.GetComponent<Bubble>().rowRaw >= maxRowHitIdx)
                    {
                        maxRowHitIdx = target.GetComponent<Bubble>().rowRaw;
                        Debug.Log("setting max row hit " + maxRowHitIdx + " bbl " + target.gameObject.name);
                    }
                    yield return target.StartCoroutine(target.Pop());
                }
            }
        }
        else
        {
            //reseteo los estados de procesado para poder hacer combo de vuelta cuando haya suficientes.
            foreach (var bb in instance.cluster)
            {
                if (bb != null)
                {
                    bb.GetComponent<PopBubble>().processed = false;
                }
            }
        }

        yield return null;

        if (instance.cluster.Count >= 3)
            yield return StartCoroutine(ProcessFloatingClusters(maxRowHitIdx));

        instance.cluster = new List<Bubble>();
    }

    /// <summary>
    /// Clusters que pueden caer en un impacto y explosion
    /// </summary>
    /// <returns></returns>
    public IEnumerator ProcessFloatingClusters(int highestRowProcessed)
    {
        Debug.Log("Processing floating clusters...." + " max idx = " + highestRowProcessed);

        List<Bubble> foundFloatingClusters = new List<Bubble>();

        List<Bubble> toProcessFloating = new List<Bubble>();


        bool isFloating = false;


        // busco desde la ultima posicion que se que explotaron burbujas,
        // todas las que estan en esa fila, y las que estan por debajo.
        for (int row = highestRowProcessed; row < instance.grid.GetLength(1); row++)
        {
            for (int column = 0; column < instance.grid.GetLength(0); column++)
            {
                var tile = instance.grid[column, row];
                if (tile == null || !tile.gameObject.activeInHierarchy)
                {
                    isFloating = true;
                    continue;
                }
                else
                {
                    toProcessFloating.Add(tile);
                }
                //instance.grid[column, row].GetComponent<SpriteRenderer>().color = Color.black; //dbg

            }
        }

        yield return null;

        // cuando tengo esa lista armada, voy a recorrer buscando sus vecinos que esten en
        // posicion de vecino [ 0, 1, 2, 3, 4 ]
        // ( en sentido contrario de las agujas del reloj, pos. derecha a izquierda lateral )
        BubbleNeighbor neighborObject = new BubbleNeighbor();
        var listedNeighbors = neighborObject.GetUpperNeighbors();

        foreach (var possibleFloater in toProcessFloating)
        {
            if (possibleFloater.rowRaw < highestRowProcessed)
                continue;

            for (int i = 0; i < listedNeighbors.Length; i++)
            {
                var targetNeighborCol = possibleFloater.colRaw + (int)listedNeighbors[i].x;
                var targetNeighborRow = possibleFloater.rowRaw + (int)listedNeighbors[i].y;

                if (targetNeighborCol < 0)
                    targetNeighborCol = 0;
                else if (targetNeighborCol >= TileGrid.instance.grid.GetLength(0))
                    targetNeighborCol = TileGrid.instance.grid.GetLength(0) - 1;

                if (targetNeighborRow < 0)
                    targetNeighborRow = 0;
                else if (targetNeighborRow >= TileGrid.instance.grid.GetLength(1))
                    targetNeighborRow = TileGrid.instance.grid.GetLength(1) - 1;

                isFloating &= (instance.grid[targetNeighborCol, targetNeighborRow] == null ||
                    !instance.grid[targetNeighborCol, targetNeighborRow].gameObject.activeInHierarchy ||
                    instance.grid[targetNeighborCol, targetNeighborRow].floating);

                Debug.Log("floating = " + isFloating);

                if (instance.grid[targetNeighborCol, targetNeighborRow])
                {
                    instance.grid[targetNeighborCol, targetNeighborRow].floating = isFloating;
                    foundFloatingClusters.Add(instance.grid[targetNeighborCol, targetNeighborRow]);
                }
            }
        }



        foreach (var floating in foundFloatingClusters)
        {
            if (floating.gameObject == null || !floating.gameObject.activeInHierarchy)
                continue;

            Debug.Log("Popping " + floating.name);
            floating.GetComponent<SpriteRenderer>().color = Color.white;


            Debug.Log("foundFloatingClusters " + floating.gameObject.name);

            yield return floating.GetComponent<PopBubble>().StartCoroutine(floating.GetComponent<PopBubble>().StartNeighborScan(new BubbleType(), false));

        }



    }

    /// <summary>
    /// Trasnforma una coordenada aproximada del mundo a una coordenada local de grilla
    /// </summary>
    /// <param name="posToParse">Posicion a transformar</param>
    /// <returns>(X,Y)</returns>
    public Tuple<int, int> WorldToGridPosition(Vector2 posToParse, int colKnown, int rowKnown)
    {


        bool isEvenRow = rowKnown % 2 == 0;
        if (isEvenRow)
            posToParse.x += 0.1f;
        else
            posToParse.x -= 0.1f;

        int Item1 = (int)(posToParse.x / tileSize);
        int Item2 = (int)(posToParse.y / -tileSize);

        //if (isEvenRow)
        //    res.x += 0.1f;


        return new Tuple<int, int>(Item1, Item2);

    }
}
