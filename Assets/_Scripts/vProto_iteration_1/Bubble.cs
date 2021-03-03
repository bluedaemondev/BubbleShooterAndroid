using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Bubble : MonoBehaviour, IPooleableObject
{
    [Header("Tipo de burbuja cargado")]
    public BubbleType selectedColor;
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

    public bool throwableMutated;
    public Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        this.sprRend = GetComponent<SpriteRenderer>();
        TileGrid.instance.onResetProcessed.AddListener(this.ResetProcessed);
        TileGrid.instance.onRemoveCluster.AddListener(this.RemoveFromGrid);
        GameManagerActions.instance.defeatEvent.AddListener(this.DisableCollider);
        GameManagerActions.instance.winEvent.AddListener(this.DisableCollider);

    }

    #region Debugging
    void OnMouseDown()
    {
        //Debug.Log("CLICK EN ROW: " + this.rowRaw + " , COL: " + this.colRaw);
    }
    #endregion

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
        //if (this.colRaw == col && this.rowRaw == row)
        //    Destroy(this.gameObject);
        //Debug.Log("call rfg = " + col + " , " + row + " ; cnt = " + countAux);
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
        GenerateColor();
        GenerateNewCoords(row, col, tileSize);
    }
    private void GenerateColor()
    {
        int rndColor = Random.Range(0, BubbleResources.instance.bubbleResources.Count);

        //this.sprRend.color = colorBubbles[rndColor];
        this.selectedColor = BubbleResources.instance.bubbleResources[rndColor];
        this.sprRend.sprite = selectedColor.sprite;

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
            this.processed = true;

        }


    }


    void HandleSnapIntoGrid(Vector2 posHitAprox, int colHit, int rowHit)
    {
        throwableMutated = false;
        


        transform.parent = TileGrid.instance.transform;

        if (colHit < 0)
            colHit = 0;
        else if (colHit >= TileGrid.instance.cols)
            colHit = TileGrid.instance.cols - 1;

        if (rowHit < 0)
            rowHit = 0;
        else if (rowHit >= TileGrid.instance.rows)
            rowHit = TileGrid.instance.rows - 1;

        //bool addTile = false;
        var radius = TileGrid.instance.tileSize / 2;

        if (TileGrid.instance.grid[colHit, rowHit] != null)
        {
            var hitBubble = TileGrid.instance.grid[colHit, rowHit];

            Debug.Log("Hit handling at x:" + hitBubble.colRaw + ", y: " + hitBubble.rowRaw);

            BubbleNeighbor neighborComparer = new BubbleNeighbor();
            var listNeighborOffset = neighborComparer.GetTileOffsetsBasedOnParity(rowHit % 2);

            #region por posicion (no anda)
            //// Cuadrante A y D
            //// presente.x - radio >= entrante.x
            //if (hitBubble.transform.position.x - radius >= transform.position.x)
            //{
            //    //LADO IZQUIERDO

            //    // Camino cuadrante D
            //    if (hitBubble.transform.position.x - radius >= transform.position.y)
            //    {
            //        Debug.Log("About to snap to Quadrant D : (" + colHit + ", " + rowHit + ")");

            //        // Desplazamiento abajo / abajo a la izquierda
            //        colHit = colHit + (int)listNeighborOffset[4].x;
            //        rowHit = rowHit + (int)listNeighborOffset[4].y;

            //        Debug.Log("Snap to Quadrant D : (" + colHit + ", " + rowHit + ")");
            //    }
            //    // Camino cuadrante A
            //    else if(hitBubble.transform.position.x + radius <= transform.position.y)
            //    {
            //        Debug.Log("About to snap to Quadrant A : (" + colHit + ", " + rowHit + ")");
            //        // Desplazamiento arriba / arriba a la izquierda
            //        colHit = colHit + (int)listNeighborOffset[2].x;
            //        rowHit = rowHit + (int)listNeighborOffset[2].y;

            //        Debug.Log("Snap to Quadrant A : (" + colHit + ", " + rowHit + ")");
            //    }
            //    else
            //    {
            //        Debug.Log("Algo que no tendria que pasar : (" + colHit + ", " + rowHit + ")");

            //    }
            //}
            //// Cuadrante B y C, e intermedios indefinidos
            //else
            //{
            //    // Camino cuadrante B
            //    if (hitBubble.transform.position.x + TileGrid.instance.tileSize / 2 <= transform.position.y)//<= transform.position.y)
            //    {
            //        Debug.Log("About to snap to Quadrant B : (" + colHit + ", " + rowHit + ")");

            //        // Desplazamiento arriba a la derecha
            //        colHit = colHit + (int)listNeighborOffset[1].x;
            //        rowHit = rowHit + (int)listNeighborOffset[1].y;

            //        Debug.Log("Snap to Quadrant B : (" + colHit + ", " + rowHit + ")");
            //    }
            //    // Camino cuadrante D
            //    else if (hitBubble.transform.position.x - TileGrid.instance.tileSize / 2 >= transform.position.y)
            //    {
            //        Debug.Log("About to snap to Quadrant D : (" + colHit + ", " + rowHit + ")");

            //        // Desplazamiento abajo a la derecha
            //        colHit = colHit + (int)listNeighborOffset[5].x;
            //        rowHit = rowHit + (int)listNeighborOffset[5].y;
            //        Debug.Log("About to snap to Quadrant D : (" + colHit + ", " + rowHit + ")");

            //    }
            //    // Medios a la derecha e izquierda
            //    else
            //    {
            //        if (hitBubble.transform.position.x + TileGrid.instance.tileSize / 2 <= transform.position.x)
            //        {
            //            Debug.Log("About to snap to MED-R : (" + colHit + ", " + rowHit + ")");

            //            // Desplazamiento derecha
            //            colHit = colHit + (int)listNeighborOffset[0].x;
            //            rowHit = rowHit + (int)listNeighborOffset[0].y;
            //            Debug.Log("Snap to MED-R : (" + colHit + ", " + rowHit + ")");

            //        }
            //        else
            //        {
            //            Debug.Log("About to snap to MED-L : (" + colHit + ", " + rowHit + ")");

            //            // Desplazamiento izquierda
            //            colHit = colHit + (int)listNeighborOffset[3].x;
            //            rowHit = rowHit + (int)listNeighborOffset[3].y;
            //            Debug.Log("Snap to MED-L : (" + colHit + ", " + rowHit + ")");

            //        }
            //    }

            //}
            #endregion
            bool foundPos = false;
            for (int neighbor = 0; neighbor < listNeighborOffset.Length; neighbor++)
            {
                var cast = Physics2D.CircleCastAll(hitBubble.transform.position, radius, listNeighborOffset[neighbor]);
                //List<Bubble> bjkl = new List<Bubble>();

                //foreach (var found in cast)
                //    bjkl.Add(found.collider.GetComponent<Bubble>());

                //Debug.Log(cast.Length);

                foreach (var found in cast)
                    if (found.collider.gameObject != gameObject)
                    {
                        Debug.Log("found bubble at neighbor offset " + listNeighborOffset[neighbor].ToString());

                        colHit = hitBubble.GetComponent<Bubble>().colRaw + (int)listNeighborOffset[neighbor].x;
                        rowHit = hitBubble.GetComponent<Bubble>().rowRaw + (int)listNeighborOffset[neighbor].y; //hitbubble
                        foundPos = true;
                        break;
                    }

                if (foundPos)
                    break;
            }
            Destroy(this.rb);
            gameObject.layer = LayerMask.NameToLayer("attachTo");
        }

        this.colRaw = colHit;
        this.rowRaw = rowHit;

        GenerateNewCoords(rowRaw, colRaw, TileGrid.instance.tileSize);

        //TileGrid.instance.cluster = TileGrid.instance.GetCluster(colHit, rowHit, true);

        TileGrid.instance.grid[colRaw, rowRaw] = this;

        if (GameManagerActions.instance.CheckGameOver())
            return;

        //TileGrid.instance.SetCurrentCluster(colHit, rowHit, true, true);
    }
    #endregion
}
