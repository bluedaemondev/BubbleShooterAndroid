/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class Common {

    public enum BallColors{
        None,
        Red,
        Blue, 
        Green, 
        Yellow, 
        Pink
    }

    public enum GameState
    {
        Playing, 
        Gameover 
    }
    public delegate void SimpleEvent ();
    public delegate void SimpleEventIntegerParams (int param);


    public const string LAYER_BALL = "Ball";
    public const string LAYER_BULLET = "Bullet";
    public const string LAYER_WALL = "Wall";
    public const string LAYER_WALL_LINE = "WallForAimingLine";
    public const string LAYER_DEADLINE = "Deadline";
    public const string LAYER_NONE = "NoInteraction";


}
