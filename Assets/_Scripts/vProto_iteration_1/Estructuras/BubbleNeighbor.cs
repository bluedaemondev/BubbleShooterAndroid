using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleNeighbor
{
    Vector2[,] neighborOffsets;

    public BubbleNeighbor()
    {
        this.neighborOffsets = new Vector2[2,7] {
            {
                    // Filas pares tile offsets
                new Vector2(1, 0) ,
                new Vector2(-1,0),
                new Vector2(0, 1) ,
                new Vector2(1, 1),
                //new Vector2(0,0),
                new Vector2(-1,1),
                //new Vector2(0,0),
                new Vector2(-1,-1),
                //new Vector2(0,0),
                new Vector2(0,-1)
            },
            {
                    // Filas impares tile offsets
                new Vector2(1, 0),
                new Vector2(1, 1),
                //new Vector2(0,0),
                new Vector2(-1,0),
                new Vector2(0,1),
                new Vector2(-1,1),
                //new Vector2(0,0),
                new Vector2(0,-1),
                new Vector2(1,-1)
                //new Vector2(0,0)
            }
        };
    }

    public Vector2[] GetUpperNeighbors()
    {
        Vector2[] result = new Vector2[3];
        result[0] = neighborOffsets[0, 2];
        result[1] = neighborOffsets[0, 3];
        result[2] = neighborOffsets[0, 4];

        return result;
    }

    public Vector2[] GetWithoutDiagonals()
    {
        Vector2[] result = new Vector2[4];
        result[0] = neighborOffsets[0, 0];
        result[1] = neighborOffsets[0, 1];
        result[2] = neighborOffsets[0, 2];
        result[3] = neighborOffsets[0, 6];


        return result;
    }

    /// <summary>
    /// Devuelve los offsets que tiene segun la paridad de la fila
    /// </summary>
    /// <param name="moduleRow">Numero de fila % 2</param>
    /// <returns>[ [CoordX, CoordY], [CoordX, CoordY], [CoordX, CoordY] ...  ]</returns>
    public Vector2[] GetTileOffsetsBasedOnParity(int moduleRow)
    {
        Vector2[] results = new Vector2[neighborOffsets.GetLength(1)];
        if(moduleRow == 0)
        {
            for(int i = 0; i<results.Length; i++)
            {
                results[i] = neighborOffsets[0, i];
            }
        }
        else
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = neighborOffsets[1, i];
            }
        }
        return results;
    }
}
