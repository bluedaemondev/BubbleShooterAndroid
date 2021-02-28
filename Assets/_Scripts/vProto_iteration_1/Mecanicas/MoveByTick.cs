using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTick : MonoBehaviour
{
    public bool useTileGridTilesize = true;

    void Start()
    {
        TickSystem.instance.onTurnPassed.AddListener(this.MoveSelf);
    }
    void MoveSelf()
    {
        if (useTileGridTilesize)
        {
            transform.position += new Vector3(0f, -TileGrid.instance.tileSize, 0f);
        }
    }
}
