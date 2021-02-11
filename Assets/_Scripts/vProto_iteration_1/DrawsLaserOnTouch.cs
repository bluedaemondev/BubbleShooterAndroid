using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawsLaserOnTouch : MonoBehaviour
{
    public LineRenderer lineRend;
    public string bounceTag = "bouncer";//tag it can reflect off.
    public float maxDist = 100;//max distance for beam to travel.
    public int maxSplitCount = 5;

    Vector2 lastMousePos;

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, transform.position);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            var dirMouse = (transform.position - mousePos).normalized * maxDist;

            if (lineRend.positionCount < 2)
                lineRend.positionCount = 2;

            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, mousePos);


            DrawLaserReflection(this.transform.position, -dirMouse, maxSplitCount);

        }
        else
        {
            lineRend.positionCount = 1;

        }
    }

    void DrawLaserReflection(Vector2 position, Vector2 direction, int reflectRemaining)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, direction, maxDist);

        if (hit2D)
        {
            Debug.Log("hitting something");
            lineRend.positionCount = maxSplitCount - reflectRemaining + 2; //+ 3
            Debug.Log(maxSplitCount - reflectRemaining + 2 + " ,, ref rem " + reflectRemaining ); //+ 1
            lineRend.SetPosition(reflectRemaining, hit2D.point); //+ 1

            if (hit2D.transform.gameObject.CompareTag(bounceTag))
            {
                Debug.Log("bouncing at " + hit2D.point);
                direction = Vector2.Reflect(direction, hit2D.normal);
                position = hit2D.point + direction * 0.01f;

                if (reflectRemaining > 0)
                {
                    DrawLaserReflection(position, direction, --reflectRemaining);
                }
            }
        }
    }

    //void DrawLaser()
    //{
    //    vertexIdx = 2;
    //    iactive = true;
    //    currentRot = transform.up;
    //    //currentPos = transform.localPosition;
    //    currentPos = transform.position;

    //    lineRend.positionCount = 2;
    //    //lineRend.SetPosition(0, transform.localPosition);
    //    lineRend.SetPosition(0, transform.position);
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;

    //    lineRend.SetPosition(1, mousePos - transform.position);

    //    while (iactive)
    //    {
    //        vertexIdx++;
    //        RaycastHit rHit = new RaycastHit();
    //        lineRend.positionCount = vertexIdx;
    //        Debug.DrawRay(currentPos, (lineRend.GetPosition(1) - currentPos).normalized * maxDist, Color.green);
    //        Debug.Log(Physics.Raycast(currentPos, (lineRend.GetPosition(1) - currentPos).normalized * maxDist, out rHit));
    //        if (Physics.Raycast(currentPos, (lineRend.GetPosition(1) - currentPos).normalized * maxDist, out rHit)) //rHit, maxDist))
    //        {
    //            Debug.Log(rHit.collider.name);
    //            //verti++;
    //            currentPos = rHit.point;
    //            currentRot = Vector3.Reflect(currentRot, rHit.normal);
    //            lineRend.SetPosition(vertexIdx - 1, rHit.point);
    //            if (rHit.transform.gameObject.tag != bounceTag)
    //            {
    //                iactive = false;
    //            }
    //        }
    //        else
    //        {
    //            //verti++;
    //            iactive = false;
    //            lineRend.SetPosition(vertexIdx - 1, currentPos + 100 * currentRot);

    //        }
    //        if (vertexIdx > maxReflections)
    //        {
    //            iactive = false;
    //        }


    //    }
    //}
}

