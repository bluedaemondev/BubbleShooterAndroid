using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleNeighbor
{
    Vector2[,] neighborOffsets;

    public BubbleNeighbor()
    {
        this.neighborOffsets = new Vector2[2,6] {
            {
                    // Filas pares tile offsets
                new Vector2(1, 0) ,
                new Vector2(0, 1) ,
                new Vector2(-1,1),
                new Vector2(-1,0),
                new Vector2(-1,-1),
                new Vector2(0,-1)
            },
            {
                    // Filas impares tile offsets
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0,1),
                new Vector2(-1,0),
                new Vector2(0,-1),
                new Vector2(1,-1)
            }
        };
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
