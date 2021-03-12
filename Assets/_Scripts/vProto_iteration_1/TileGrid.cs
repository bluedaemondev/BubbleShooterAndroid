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

    public void SetCluster(bool force = false)
    {
        StartCoroutine(ClusterPopOrReset());
    }

    IEnumerator ClusterPopOrReset(bool force = false)
    {

        int maxRowHitIdx = 1 ;

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
                        maxRowHitIdx = target.GetComponent<Bubble>().rowRaw;

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

        instance.cluster = new List<Bubble>();

        yield return StartCoroutine(ProcessFloatingClusters(maxRowHitIdx));

    }

    /// <summary>
    /// Clusters que pueden caer en un impacto y explosion
    /// </summary>
    /// <returns></returns>
    public IEnumerator ProcessFloatingClusters(int highestRowProcessed) //<List<Bubble>>
    {
        Debug.Log("Processing floating clusters....");

        List<Bubble> foundFloatingClusters = new List<Bubble>();

        List<Bubble> toProcessFloating = new List<Bubble>();

        // busco desde la ultima posicion que se que explotaron burbujas,
        // todas las que estan en esa fila, y las que estan por debajo.
        for (int column = 0; column < instance.grid.GetLength(0); column++)
        {
            for (int row = highestRowProcessed; row < instance.grid.GetLength(1); row++)
            {
                Debug.Log("fallando en " + column + " " + row);
                var tile = instance.grid[column, row];
                if (tile == null || !tile.gameObject.activeInHierarchy)
                    continue;

                toProcessFloating.Add(tile);
            }
        }

        yield return null;

        // cuando tengo esa lista armada, voy a recorrer buscando sus vecinos que esten en
        // posicion de vecino [ 0, 1, 2, 3 ]
        // ( en sentido contrario de las agujas del reloj, pos. derecha a izquierda lateral )
        BubbleNeighbor neighborObject = new BubbleNeighbor();
        var listedNeighbors = neighborObject.GetTileOffsetsBasedOnParity(highestRowProcessed % 2);

        bool isFloating = false;

        for (int posN = 0; posN < 4; posN++)
        {
            switch (posN)
            {
                case 1: // arriba derecha (impar) / arriba (par)
                case 2: // arriba (impar) / arriba izquierda (par)
                    foreach (var tile in toProcessFloating.FindAll(i => i.colRaw == highestRowProcessed))
                    {
                        var targetNeighborCol = tile.colRaw + (int)listedNeighbors[posN].x;
                        //Debug.Log("fallando 2 en " + targetNeighborCol + " col");

                        if (targetNeighborCol < instance.grid.GetLength(0) &&
                            instance.grid[targetNeighborCol, highestRowProcessed] == null)
                        {
                            isFloating = true;
                            //Debug.Log(" floating cluster below" + instance.grid[targetNeighborCol, highestRowProcessed]);
                        }
                    }
                    break;
            }

            if (isFloating)
                foundFloatingClusters.AddRange(toProcessFloating);

            yield return null;
        }

        foreach (var floating in foundFloatingClusters)
        {
            Debug.Log("Popping " + floating.name);
            yield return floating.GetComponent<PopBubble>()?.StartCoroutine(floating.GetComponent<PopBubble>().Pop());

        }

        Debug.Log(foundFloatingClusters);


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
