/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using System;
[Serializable]
public class LevelProfile  {

    public int _timeToPushDown;
    public int GetTimeToPushDown(){
        return Mathf.Clamp(_timeToPushDown,1,100);
    }

    public int _initRows;
    public int GetInitRow(){
        return Mathf.Clamp(_initRows,1,12);
    }

    public int _numberOfDifferentColors;
    public int GetNumColor(){
        return Mathf.Clamp(_numberOfDifferentColors,2,5);
    }
}
