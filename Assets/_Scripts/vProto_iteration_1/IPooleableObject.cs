using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooleableObject 
{
    void OnObjectSpawn(int row, int col, float tileSize);
    
}
