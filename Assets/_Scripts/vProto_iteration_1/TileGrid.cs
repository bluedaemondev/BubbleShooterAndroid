using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGrid : MonoBehaviour
{
    public static TileGrid instance { get; private set; }

    public int rows = 20;
    public int cols = 11;
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

        onRemoveCluster.AddListener(RemClusTest); // delete
    }

    // Start is called before the first frame update
    void Start()
    {
        AdmobComponentsManager.instance.onSendToBackAds.Invoke();
    }

    private void GenerateGrid()
    {
        instance.grid = new Bubble[cols, rows + 10];

        for (int row = 0; row < rows; row++)
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

        float gridWidth = cols * tileSize;
        float gridHeight = rows * tileSize;

        transform.position = new Vector3(-2.5f, gridHeight * tileSize * 2.75f);
    }
    public List<Bubble> GetNeighbors(Bubble tile)
    {
        var nArr = neighborOffsetArray.GetTileOffsetsBasedOnParity(tile.rowRaw % 2);
        List<Bubble> neighbors = new List<Bubble>();

        for (int pos = 0; pos < nArr.Length; pos++)
        {
            // coordenada del vecino
            var nXpos = (int)(tile.colRaw + nArr[pos].x);
            var nYpos = (int)(tile.rowRaw + nArr[pos].y);

            // validacion de datos
            if (nXpos >= 0 && nXpos < instance.cols && nYpos >= 0 && nYpos < instance.rows)
            {
                neighbors.Add(grid[nXpos, nYpos]);
            }
        }
        //Debug.Log("neighbors found: ");
        //foreach (var n in neighbors)
        //    Debug.Log(n.ToString() + ", row " + n.rowRaw + ", col " + n.colRaw + " , color = " + n.selectedColor.ToString());
        //Debug.Log("end");

        return neighbors;
    }

    /// <summary>
    /// Devuelve un cluster de burbujas en la posicion indicada
    /// </summary>
    /// <param name="tileX"></param>
    /// <param name="tileY"></param>
    /// <returns></returns>
    public List<Bubble> GetCluster(int tileX, int tileY, bool matchColor, bool reset)
    {
        List<Bubble> foundCluster = new List<Bubble>();

        Bubble targetTile = grid[tileX, tileY];
        Stack<Bubble> toProcess = new Stack<Bubble>();

        toProcess.Push(targetTile);

        while (toProcess.Count > 0)
        {
            var currentTile = toProcess.Pop();
            // si es del mismo el color o no hace falta que sea match por color
            if (!matchColor || currentTile.type.type == targetTile.type.type)
            {
                foundCluster.Add(currentTile);
                var neighbors = GetNeighbors(currentTile);

                // reviso los colores vecinos
                for (var i = 0; i < neighbors.Count; i++)
                {
                    if (!neighbors[i].processed)
                    {
                        toProcess.Push(neighbors[i]);
                        neighbors[i].processed = true;
                    }
                }
            }
        }

        return foundCluster;
    }

    /// <summary>
    /// Clusters que pueden caer en un impacto y explosion
    /// </summary>
    /// <returns></returns>
    public List<Bubble> GetFloatingClusters()
    {
        List<Bubble> foundFloatingClusters = new List<Bubble>();
        onResetProcessed.Invoke();

        //reviso todas las burbujas
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var tile = grid[i, j];
                if (!tile.processed)
                {
                    var foundCluster = GetCluster(i, j, false, false);

                    // tiene que haber al menos un tile en el cluster
                    if (foundCluster.Count <= 0)
                        continue;

                    var floating = true;
                    for (var k = 0; k < foundCluster.Count; k++)
                    {
                        if (foundCluster[k].rowRaw == 0)
                        {
                            // esta pegado al techo / ultima fila cargada
                            floating = false;
                            break;
                        }
                    }
                    if (floating)
                    {
                        foundFloatingClusters.AddRange(foundCluster);
                    }
                }
            }
        }

        Debug.Log(foundFloatingClusters);

        return foundFloatingClusters;

    }

    public void SetCurrentCluster(int colHit, int rowHit, bool matchColor, bool reset)
    {
        instance.cluster = GetCluster(colHit, rowHit, matchColor, reset);
        if (cluster.Count >= 3)
        {
            onRemoveCluster.Invoke(colHit, rowHit, cluster.Count); // paso la cantidad de burbujas al contador de puntos
            foreach (var i in GetFloatingClusters())
            {
                Destroy(i);
            }
            return;
        }
        // contador por turnos? o seguir con el tiempo fijo?

        /*
         turncounter++;
        if (turncounter >= 5) {
            // Add a row of bubbles
            addBubbles();
            turncounter = 0;
            rowoffset = (rowoffset + 1) % 2;
            
            if (checkGameOver()) {
                return;
            }
        }
        */
    }
    public void RemClusTest(int column, int row, int count)
    {
        while (cluster.Count > 0)
        //foreach (var it in cluster)
        {
            if (column == cluster[cluster.Count - 1].colRaw && row == cluster[cluster.Count - 1].rowRaw)
            {
                Debug.Log("call RemClusTest ev. = " + cluster[cluster.Count - 1].colRaw + " , " + cluster[cluster.Count - 1].rowRaw + " ; cnt = " + count);
                Destroy(cluster[cluster.Count - 1].gameObject);
                Debug.Log("removing row: " + (cluster[cluster.Count - 1].rowRaw + " col: " + (cluster[cluster.Count - 1].colRaw)));
                cluster.RemoveAt(cluster.Count - 1);
            }
        }
        //while (floatingclusters.Count > 0)
        //{
        //    Destroy(floatingclusters[floatingclusters.Count - 1]);
        //    floatingclusters.Remove(floatingclusters[floatingclusters.Count - 1]);

        //}
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
