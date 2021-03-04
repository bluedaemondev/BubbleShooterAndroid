using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownBubble : MonoBehaviour
{
    Vector2 lastVelocity;
    Rigidbody2D rbBubble;
    BubbleType type;

    private void Start()
    {
        rbBubble = GetComponent<Rigidbody2D>();
        ConstructThrowableBubble();
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
        gameObject.layer = LayerMask.NameToLayer("Default");

        this.type = BubbleResources.GenerateRandomBubbleType();
    }

    public void ProcessHit(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("bounce"))
        {
            HandleBounce(collisionInfo);
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("attachTo"))
        {
            var bubbleCollisionWith = collisionInfo.collider.GetComponent<Bubble>();
            HandleSnapIntoGrid(bubbleCollisionWith);
            //this.processed = true;
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
        else if (colHit >= TileGrid.instance.cols)
            colHit = TileGrid.instance.cols - 1;

        if (rowHit < 0)
            rowHit = 0;
        else if (rowHit >= TileGrid.instance.rows)
            rowHit = TileGrid.instance.rows - 1;

        var radius = TileGrid.instance.tileSize / 2;

        if (TileGrid.instance.grid[colHit, rowHit] != null)
        {
            var hitBubble = TileGrid.instance.grid[colHit, rowHit];

            Debug.Log("Hit handling at x:" + hitBubble.colRaw + ", y: " + hitBubble.rowRaw);

            BubbleNeighbor neighborComparer = new BubbleNeighbor();
            var listNeighborOffset = neighborComparer.GetTileOffsetsBasedOnParity(rowHit % 2);


            var cast = Physics2D.RaycastAll(hitBubble.transform.position, (transform.position - hitBubble.transform.position), radius * 2f);
            Debug.DrawRay(hitBubble.transform.position, (transform.position - hitBubble.transform.position) * radius * 2f, Color.green, 2f);

            foreach (var found in cast)
                if (found.collider.GetComponent<ThrownBubble>())
                {
                    Debug.Log("found bubble at  " + found.collider.name.ToString() + " , " + found.point);
                    SetNearestPositionOnGrid(listNeighborOffset, hitBubble.transform, transform);
                    break;
                }

            //Destroy(this.gameObject);
           
        }



        //TileGrid.instance.cluster = TileGrid.instance.GetCluster(colHit, rowHit, true);

        if (GameManagerActions.instance.CheckGameOver())
            return;

        //TileGrid.instance.SetCurrentCluster(colHit, rowHit, true, true);
        this.gameObject.SetActive(false);
    }

    private void SetNearestPositionOnGrid(Vector2[] offsets, Transform origin, Transform targetToAdapt)
    {
        int minDistIdx = 0;
        float minDist = 9999999;

        for (int i = 0; i < offsets.Length; i++)
        {
            var distance = Vector2.Distance((Vector2)origin.position + offsets[i] * TileGrid.instance.tileSize, targetToAdapt.position);
            if (distance <= minDist)
            {
                minDist = distance;
                minDistIdx = i;
            }
        }

        var swapObject = ObjectPooler.instance.SpawnFromPool("bubble");

        swapObject.GetComponent<Bubble>().type = this.type;

        swapObject.transform.position = (Vector2)origin.transform.position + offsets[minDistIdx] * TileGrid.instance.tileSize;
        swapObject.GetComponent<Bubble>().colRaw = (int)offsets[minDistIdx].x + origin.GetComponent<Bubble>().colRaw;
        swapObject.GetComponent<Bubble>().rowRaw = (int)offsets[minDistIdx].y + origin.GetComponent<Bubble>().rowRaw;

        swapObject.layer = LayerMask.NameToLayer("attachTo");
        
        TileGrid.instance.grid[swapObject.GetComponent<Bubble>().colRaw, swapObject.GetComponent<Bubble>().rowRaw] =
            swapObject.GetComponent<Bubble>();

    }

}
