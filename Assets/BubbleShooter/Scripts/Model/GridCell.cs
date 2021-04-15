/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class GridCell  {

    public int X;
    public int Y;
    public Vector3 Position;
    public Ball Ball;

    public GridCell (int gridX, int gridY, Vector3 realPos){
        X = gridX;
        Y = gridY;
        Position = realPos;
    }

    public GridCell (int gridX, int gridY){
        X = gridX;
        Y = gridY;
    }
}
