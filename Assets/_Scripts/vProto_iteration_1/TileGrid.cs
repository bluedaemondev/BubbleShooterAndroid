using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public int rows = 22;
    public int cols = 11;
    public float tileSize = 0.5f;

    public BubbleNeighbor neighborOffsetArray;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject refTile = (GameObject)ObjectPooler.instance.SpawnFromPool("bubble");
                refTile.GetComponent<Bubble>().OnObjectSpawn(row, col, tileSize);
            }
        }

        float gridWidth = cols * tileSize;
        float gridHeight = rows * tileSize;

        // divido por 2 por el pivot point (que esta en el centro)
        //transform.position = new Vector3(-gridWidth * tileSize + tileSize/2, tileSize * gridHeight * 2);//* tileSize / 2);
        transform.position = new Vector3(-2.5f, gridHeight * tileSize * 2.75f);
        //-gridWidth * tileSize * 5
    }

}
