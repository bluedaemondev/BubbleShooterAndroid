using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bubble))]
public class PopBubble : MonoBehaviour
{
    Bubble compoBubble;

    public void Pop()
    {
        // destruir y mandar al pool
        // mostrar particulas
    }

    List<Bubble> FindBubbleCluster(int tileX, int tileY, Color matchColor)
    {
        List<Bubble> result = new List<Bubble>();

        return result;
    }

    /*
     function getNeighbors(tile) {
        var tilerow = (tile.y + rowoffset) % 2; // Even or odd row
        var neighbors = [];
        
        // Get the neighbor offsets for the specified tile
        var n = neighborsoffsets[tilerow];
        
        // Get the neighbors
        for (var i=0; i<n.length; i++) {
            // Neighbor coordinate
            var nx = tile.x + n[i][0];
            var ny = tile.y + n[i][1];
            
            // Make sure the tile is valid
            if (nx >= 0 && nx < level.columns && ny >= 0 && ny < level.rows) {
                neighbors.push(level.tiles[nx][ny]);
            }
        }
        
        return neighbors;
    }
     */
    public void SearchForNeighborBubbles()
    {
        // raycast hacia 4 direcciones
        var grid = FindObjectOfType<TileGrid>();
        for (int y = 0; y < grid.rows; y++)
        {
            for (int x = 0; x < grid.cols; x++)
            {

            }
        }
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
