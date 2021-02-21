using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileGrid : MonoBehaviour
{
    public static TileGrid instance { get; private set; }

    public int rows = 22;
    public int cols = 11;
    public float tileSize = 0.5f;

    public BubbleNeighbor neighborOffsetArray;
    public Bubble[,] grid;
    public List<Bubble> cluster;
    public List<Bubble> floatingclusters;

    public UnityEvent<int> onRemoveCluster;
    public UnityEvent onResetProcessed;


    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        neighborOffsetArray = new BubbleNeighbor();
        onRemoveCluster = new UnityEvent<int>();
        onResetProcessed = new UnityEvent();

        cluster = new List<Bubble>();
        floatingclusters = new List<Bubble>();

        onRemoveCluster.AddListener(RemClusTest); // delete
        //onRemoveCluster.AddListener()
    }

    private void GenerateGrid()
    {
        this.grid = new Bubble[cols, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject refTile = (GameObject)ObjectPooler.instance.SpawnFromPool("bubble");
                var bubble = refTile.GetComponent<Bubble>();
                bubble.OnObjectSpawn(row, col, tileSize);
                grid[col, row] = bubble;

            }
        }

        float gridWidth = cols * tileSize;
        float gridHeight = rows * tileSize;

        // divido por 2 por el pivot point (que esta en el centro)
        //transform.position = new Vector3(-gridWidth * tileSize + tileSize/2, tileSize * gridHeight * 2);//* tileSize / 2);
        transform.position = new Vector3(-2.5f, gridHeight * tileSize * 2.75f);
        //-gridWidth * tileSize * 5
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
            if (nXpos >= 0 && nXpos < this.cols && nYpos >= 0 && nYpos < this.rows)
            {
                neighbors.Add(grid[nXpos, nYpos]);
            }
        }

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
            if (!matchColor || currentTile.selectedColor == targetTile.selectedColor)
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

        return foundFloatingClusters;

    }

    public void SetCurrentCluster(int colHit, int rowHit, bool matchColor, bool reset)
    {
        this.cluster = GetCluster(colHit, rowHit, matchColor, reset);
        if (cluster.Count >= 3)
        {
            onRemoveCluster.Invoke(cluster.Count); // paso la cantidad de burbujas al contador de puntos
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
    public void RemClusTest(int val)
    {
        Debug.Log(val + " burbujas explotadas");


        while (cluster.Count > 0)
        //foreach (var it in cluster)
        {
            Destroy(cluster[cluster.Count - 1].gameObject);
            cluster.RemoveAt(cluster.Count - 1);
            if (floatingclusters.Count > 0)
                floatingclusters.Remove(floatingclusters[floatingclusters.Count-1]);
        }
    }
}
