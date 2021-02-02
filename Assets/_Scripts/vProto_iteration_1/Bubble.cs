using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Colores disponibles")]
    public List<Color> colorBubbles = new List<Color>
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta
    };
    
    // guardo la posicion en fila y columna al generarla/impactar en una posicion
    float colX;
    float rowY;


    public void GenerateNewCoords()
    {
        this.colX = transform.position.x;
        this.rowY = transform.position.y;

    }


    public void GetHit()
    {
        Debug.Log(this.gameObject.name + " , got hit");
        //throw new NotImplementedException();
    }
}
