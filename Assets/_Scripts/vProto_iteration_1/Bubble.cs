using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : MonoBehaviour, IPooleableObject
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
    public SpriteRenderer sprRend;

    void Start()
    {
        //this.sprRend = GetComponent<SpriteRenderer>();
    }

    public void GenerateNewCoords(int row, int col, float tileSize)
    {
        this.rowY = col * tileSize;
        this.colX = row * -tileSize;

        this.transform.position = new Vector3(rowY, colX);
    }


    public void GetHit()
    {
        Debug.Log(this.gameObject.name + " , got hit");
        //throw new NotImplementedException();
    }

    public void OnObjectSpawn(int row, int col, float tileSize)
    {
        GenerateColor();
        GenerateNewCoords(row, col, tileSize);
    }

    private void GenerateColor()
    {
        int rndColor = Random.Range(0, colorBubbles.Count);
        
        this.sprRend.color = colorBubbles[rndColor];

    }
}
