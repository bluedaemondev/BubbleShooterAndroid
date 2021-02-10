using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawsLaserOnTouch : MonoBehaviour
{
    public int vertexIdx = 1;
    bool iactive = true;
    Vector3 currentRot;
    Vector3 currentPos;

    public LineRenderer lineRend;
    public float maxDist = 10;//max distance for beam to travel.
    public string bounceTag = "bouncer";//tag it can reflect off.
    public int maxReflections = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DrawLaser();
        }
        else
        {
            lineRend.positionCount = 1;
            vertexIdx = 1;


        }
    }

    void DrawLaser()
    {
        vertexIdx = 2;
        iactive = true;
        currentRot = transform.up;
        currentPos = transform.localPosition;
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, transform.localPosition);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        lineRend.SetPosition(1, mousePos - transform.position);

        while (iactive)
        {
            vertexIdx++;
            RaycastHit rHit = new RaycastHit();
            lineRend.positionCount = vertexIdx;
            if (Physics.Raycast(currentPos, currentRot, out rHit)) //rHit, maxDist))
            {
                //verti++;
                currentPos = rHit.point;
                currentRot = Vector3.Reflect(currentRot, rHit.normal);
                lineRend.SetPosition(vertexIdx - 1, rHit.point);
                if (rHit.transform.gameObject.tag != bounceTag)
                {
                    iactive = false;
                }
            }
            else
            {
                //verti++;
                iactive = false;
                lineRend.SetPosition(vertexIdx - 1, currentPos + 100 * currentRot);

            }
            if (vertexIdx > maxReflections)
            {
                iactive = false;
            }


        }
    }
}

