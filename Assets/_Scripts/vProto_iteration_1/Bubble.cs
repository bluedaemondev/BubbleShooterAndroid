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
    public Color selectedColor;
    public bool processed;

    // posicion en coordenadas reales.
    float RowYPosition;
    float ColXPosition;

    // almacenamiento de las posiciones como las genera el mapa.
    public int colRaw;
    public int rowRaw;

    public SpriteRenderer sprRend;
    Vector3 lastVelocity;
    public Rigidbody2D rb;

    bool throwableMutated;

    void Start()
    {
        this.sprRend = GetComponent<SpriteRenderer>();
        TileGrid.instance.onResetProcessed.AddListener(this.ResetProcessed);
        TileGrid.instance.onRemoveCluster.AddListener(this.RemoveFromGrid);
    }

    void ResetProcessed()
    {
        this.processed = false;
    }
    void RemoveFromGrid(int aux)
    {
        Destroy(this.gameObject);
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
        GenerateColor();
        GenerateNewCoords(row, col, tileSize);
    }
    private void GenerateColor()
    {
        int rndColor = Random.Range(0, colorBubbles.Count);

        this.sprRend.color = colorBubbles[rndColor];
        this.selectedColor = this.sprRend.color;

    }

    #region ThrowableMutation

    void Update()
    {
        if (!throwableMutated)
            return;

        lastVelocity = rb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || !throwableMutated)
            return;

        ProcessHit(collision);
    }

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
        throwableMutated = true;
        rb = rbAdded;
        gameObject.layer = LayerMask.NameToLayer("Default");
        // algo mas ?
    }
    public void ProcessHit(Collision2D collisionInfo)
    {
        //void HandleBounceOnBound => puede rebotar
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("bounce"))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collisionInfo.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            Debug.Log(this.gameObject.name + " , bouncing");
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("attachTo"))
        {
            var hitCollider = collisionInfo.collider;
            HandleSnapIntoGrid(hitCollider.ClosestPoint(transform.position), hitCollider.GetComponent<Bubble>().colRaw, hitCollider.GetComponent<Bubble>().rowRaw);
        }


    }


    void HandleSnapIntoGrid(Vector2 posHitAprox, int colHit, int rowHit)
    {
        throwableMutated = false;
        Destroy(this.rb);
        gameObject.layer = LayerMask.NameToLayer("attachTo");


        transform.parent = TileGrid.instance.transform;

        if (colHit < 0)
            colHit = 0;
        else if (colHit >= TileGrid.instance.cols)
            colHit = TileGrid.instance.cols - 1;

        if (rowHit < 0)
            rowHit = 0;
        else if (rowHit >= TileGrid.instance.rows)
            rowHit = TileGrid.instance.rows - 1;

        //for (int newRow = rowHit + 1; newRow < TileGrid.instance.rows; newRow++)
        //{
        //if (TileGrid.instance.grid[colHit, newRow])
        TileGrid.instance.grid[colHit, rowHit] = this;
        if (GameManagerActions.instance.CheckGameOver())
            return;

        //TileGrid.instance.cluster = TileGrid.instance.GetCluster(colHit, rowHit, true);
        TileGrid.instance.SetCurrentCluster(colHit, rowHit, true, true);

        //}




    }
    #endregion
}
