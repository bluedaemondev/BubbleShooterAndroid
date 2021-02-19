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

    public void GenerateNewCoords(int row, int col, float tileSize)
    {
        this.rowY = col * tileSize;
        

        this.colX = row * -tileSize;
        
        if (row % 2 == 0)
            rowY += 0.1f;
        else
            rowY -= 0.1f;

        this.transform.position = new Vector3(rowY, colX);
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

    #region ThrowableMutation
    public void SetImpulseForce(Vector3 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    

    public void GenerateThrowableBubble()
    {
        GenerateColor();
        Rigidbody2D rbAdded = this.gameObject.AddComponent<Rigidbody2D>();
        rbAdded.gravityScale = 0;
        rbAdded.constraints = RigidbodyConstraints2D.FreezeRotation;
        // algo mas ?
    }
    public void ProcessHit(string tag)
    {
        //if()
        Debug.Log(this.gameObject.name + " , bouncing");

    }
    #endregion
}
