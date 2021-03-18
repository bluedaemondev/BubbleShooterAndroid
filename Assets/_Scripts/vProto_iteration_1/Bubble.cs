using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Bubble : MonoBehaviour, IPooleableObject
{
    [Header("Tipo de burbuja cargado")]
    public BubbleType type;
    public bool processed;
    public bool floating;

    // posicion en coordenadas reales.
    float RowYPosition;
    float ColXPosition;

    // almacenamiento de las posiciones como las genera el mapa.
    public int colRaw;
    public int rowRaw;

    SpriteRenderer sprRend;

    void Start()
    {
        this.sprRend = GetComponent<SpriteRenderer>();
        TileGrid.instance.onResetProcessed.AddListener(this.ResetProcessed);
        TileGrid.instance.onRemoveCluster.AddListener(this.RemoveFromGrid);
        GameManagerActions.instance.defeatEvent.AddListener(this.DisableCollider);
        GameManagerActions.instance.winEvent.AddListener(this.DisableCollider);

    }

    void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    void ResetProcessed()
    {
        this.processed = false;
    }
    void RemoveFromGrid(int col, int row, int countAux)
    {
        if (this.colRaw == col && this.rowRaw == row)
        {
            Debug.Log("call rfg = " + col + " , " + row + " ; cnt = " + countAux);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void GenerateNewCoords(int row, int col, float tileSize)
    {
        this.colRaw = col;
        this.rowRaw = row;

        this.ColXPosition = col * tileSize;
        this.RowYPosition = row * -tileSize;

        if (row % 2 == 0)
            ColXPosition += 0.1f;
        else
            ColXPosition -= 0.1f;

        this.transform.localPosition = new Vector3(ColXPosition, RowYPosition);
    }

    public void OnObjectSpawn(int row, int col, float tileSize)
    {
        gameObject.SetActive(true);
        this.type = BubbleResources.GenerateRandomBubbleType();

        if (sprRend == null)
            sprRend = GetComponent<SpriteRenderer>();

        this.sprRend.sprite = type.sprite;

        GenerateNewCoords(row, col, tileSize);
    }

}
