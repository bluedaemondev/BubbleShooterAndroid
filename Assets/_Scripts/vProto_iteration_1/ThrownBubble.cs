using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownBubble : MonoBehaviour
{
    Vector2 lastVelocity;
    Rigidbody2D rbBubble;
    //[SerializeField]
    public BubbleType type;

    public bool processed = false;

    private void Start()
    {
        rbBubble = GetComponent<Rigidbody2D>();
        ConstructThrowableBubble();
    }
    private void OnEnable()
    {
        this.GetComponent<Collider2D>().enabled = true;
        processed = false;
    }

    void Update()
    {
        lastVelocity = rbBubble.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        ProcessHit(collision);
    }

    public void SetImpulseForce(Vector3 force)
    {
        rbBubble.AddForce(force, ForceMode2D.Impulse);
    }

    public void ConstructThrowableBubble()
    {
        rbBubble.gravityScale = 0;
        rbBubble.constraints = RigidbodyConstraints2D.FreezeRotation;
        rbBubble.velocity = Vector2.zero;
        rbBubble.angularVelocity = 0;

        gameObject.layer = LayerMask.NameToLayer("Default");


        this.type = BubbleResources.GenerateRandomBubbleType();
        this.GetComponent<SpriteRenderer>().sprite = type.sprite;
    }

    public void ProcessHit(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("bounce"))
        {
            HandleBounce(collisionInfo);
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("attachTo") && !processed)
        {
            processed = true;
            var bubbleCollisionWith = collisionInfo.collider.GetComponent<Bubble>();
            HandleSnapIntoGrid(bubbleCollisionWith);
            this.GetComponent<Collider2D>().enabled = false;
        }


    }

    void HandleBounce(Collision2D collisionInfo)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collisionInfo.contacts[0].normal);
        rbBubble.velocity = direction * Mathf.Max(speed, 0f);
        Debug.Log(this.gameObject.name + " , bouncing");
    }

    void HandleSnapIntoGrid(Bubble processRelativeTo)
    {

        transform.parent = TileGrid.instance.transform;
        var colHit = processRelativeTo.colRaw;
        var rowHit = processRelativeTo.rowRaw;


        if (colHit < 0)
            colHit = 0;
        else if (colHit >= TileGrid.instance.grid.GetLength(0))
            colHit = TileGrid.instance.grid.GetLength(0);

        if (rowHit < 0)
            rowHit = 0;
        else if (rowHit >= TileGrid.instance.grid.GetLength(1))
            rowHit = TileGrid.instance.grid.GetLength(1) - 1;

        var radius = TileGrid.instance.tileSize / 2;

        if (TileGrid.instance.grid[colHit, rowHit] != null)
        {
            var hitBubble = TileGrid.instance.grid[colHit, rowHit];

            //Debug.Log("Hit handling at x:" + hitBubble.colRaw + ", y: " + hitBubble.rowRaw);

            BubbleNeighbor neighborComparer = new BubbleNeighbor();
            var listNeighborOffset = neighborComparer.GetWithoutDiagonals();//GetTileOffsetsBasedOnParity(rowHit % 2);


            var cast = Physics2D.RaycastAll(hitBubble.transform.position, (transform.position - hitBubble.transform.position), radius);
            Debug.DrawRay(hitBubble.transform.position, (transform.position - hitBubble.transform.position) * radius, Color.green, 2f);

            foreach (var found in cast)
                if (found.collider.GetComponent<ThrownBubble>())
                {
                    //Debug.Log("found bubble at  " + found.collider.name.ToString() + " , " + found.point);
                    SetNearestPositionOnGrid(listNeighborOffset, hitBubble.transform, transform);
                    break;
                }
        }

        if (GameManagerActions.instance.CheckGameOver())
            return;
        
        this.gameObject.SetActive(false);

        //TileGrid.instance.SetCurrentCluster(colHit, rowHit, true, false);


    }

    private void SetNearestPositionOnGrid(Vector2[] offsets, Transform origin, Transform targetToAdapt)
    {
        int minDistIdx = 0;
        float minDist = 9999999;

        for (int i = 0; i < offsets.Length; i++)
        {
            var distance = Vector2.Distance((Vector2)origin.position + offsets[i] * TileGrid.instance.tileSize, targetToAdapt.position);
            var bbl = origin.GetComponent<Bubble>();

            var trX = bbl.colRaw + (int)offsets[i].x;
            var trY = bbl.rowRaw - (int)offsets[i].y;

            if (trX < 0)
                trX = 0;
            else if (trX >= TileGrid.instance.grid.GetLength(0))
                trX = TileGrid.instance.grid.GetLength(0) - 1;

            if (trY < 0)
                trY = 0;
            else if (trY >= TileGrid.instance.grid.GetLength(1))
                trY = TileGrid.instance.grid.GetLength(1) - 1;

            var nullCheck = TileGrid.instance.grid[trX, trY];



            if (distance < minDist && nullCheck == null)
            {
                Debug.Log("Setting min dist = " + distance + " , " + trX + " " + trY);

                minDist = distance;
                minDistIdx = i;
            }

        }




        var swapObject = ObjectPooler.instance.SpawnFromPool("bubble");

        swapObject.transform.position = (Vector2)origin.transform.position + offsets[minDistIdx] * TileGrid.instance.tileSize;

        var bubbleComponentSwap = swapObject.GetComponent<Bubble>();
        bubbleComponentSwap.colRaw = (int)offsets[minDistIdx].x + origin.GetComponent<Bubble>().colRaw;
        bubbleComponentSwap.rowRaw = -(int)offsets[minDistIdx].y + origin.GetComponent<Bubble>().rowRaw;

        if (bubbleComponentSwap.colRaw < 0)
            bubbleComponentSwap.colRaw = 0;
        else if (bubbleComponentSwap.colRaw >= TileGrid.instance.grid.GetLength(0))
            bubbleComponentSwap.colRaw = TileGrid.instance.grid.GetLength(0) - 1;

        if (bubbleComponentSwap.rowRaw < 0)
            bubbleComponentSwap.rowRaw = 0;
        else if (bubbleComponentSwap.rowRaw >= TileGrid.instance.grid.GetLength(1))
            bubbleComponentSwap.rowRaw = TileGrid.instance.grid.GetLength(1) - 1;

        /// calculo el desplazamiento que tiene fila par / impar en pantalla
        if (bubbleComponentSwap.rowRaw % 2 == 0)
            swapObject.transform.position = new Vector3(bubbleComponentSwap.transform.position.x + 0.1f, bubbleComponentSwap.transform.position.y, 0);
        else
            swapObject.transform.position = new Vector3(bubbleComponentSwap.transform.position.x - 0.1f, bubbleComponentSwap.transform.position.y, 0);


        swapObject.layer = LayerMask.NameToLayer("attachTo");


        //Debug.Log("x = " + bubbleComponentSwap.colRaw + ", y = " + bubbleComponentSwap.rowRaw);

        TileGrid.instance.grid[bubbleComponentSwap.colRaw, bubbleComponentSwap.rowRaw] = bubbleComponentSwap;

        bubbleComponentSwap.type = this.type;
        swapObject.GetComponent<SpriteRenderer>().sprite = this.type.sprite;

        Debug.Log(bubbleComponentSwap.colRaw + "x snap y" + bubbleComponentSwap.rowRaw);

        var auxPop = swapObject.GetComponent<PopBubble>();
        auxPop.StartCoroutine(auxPop.StartNeighborScan(type, !BubbleResources.instance.specialBubbleResources.Contains(type)));
    }

    

}
