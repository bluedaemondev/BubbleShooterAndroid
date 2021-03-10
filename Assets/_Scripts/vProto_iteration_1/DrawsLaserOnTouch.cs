using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawsLaserOnTouch : MonoBehaviour
{
    public LineRenderer lineRend;
    public string bounceTag = "bouncer"; //tag it can reflect off.
    public float maxDist = 100;//max distance for beam to travel.
    public int maxSplitCount = 5;

    Vector2 lastMousePos;
    bool hasChangedTouch;

    int currentBounceIndex = 0;
    Vector3 laserStartPos;



    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        laserStartPos = FindObjectOfType<BubbleShooter>().spawnPrimaryBubble.transform.position;
        SetStartingLaserPoint();
    }

    void SetStartingLaserPoint()
    {

        lineRend.positionCount = 2;
        lineRend.SetPosition(0, laserStartPos);
        lineRend.SetPosition(1, laserStartPos);
        currentBounceIndex = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (Utils.instance.GetContinuousTouch())
        {
            var dirMouse = (laserStartPos - Utils.instance.MouseToWorldWithoutZ()).normalized * maxDist;


            lineRend.SetPosition(1, laserStartPos + dirMouse);
            DrawLaserReflection(laserStartPos, dirMouse, maxSplitCount);
            currentBounceIndex = 1;
        }
        else if (Utils.instance.GetTouchEnding())
        {
            SetStartingLaserPoint();

        }
        else if (Utils.instance.GetInitialTouch())
        {
            var dirMouse = (laserStartPos - Utils.instance.MouseToWorldWithoutZ()).normalized * maxDist;
            currentBounceIndex = 1;
            lineRend.positionCount = 2;
            lineRend.SetPosition(1, laserStartPos + dirMouse);
        }
    }

    void DrawLaserReflection(Vector2 position, Vector2 direction, int reflectRemaining)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, direction, maxDist);

        if (hit2D)
        {
            //if (hit2D.collider.CompareTag("bubble"))
            //{
            //    reflectRemaining = 0;
            //    lineRend.positionCount = 2;
            //    lineRend.SetPosition(1, hit2D.transform.position);
            //    return;
            //}

            lineRend.positionCount = currentBounceIndex + 1;
            
            //Debug.Log("Punto de colision laser " + hit2D.collider.name + " , " + hit2D.collider.transform.position);
            lineRend.SetPosition(currentBounceIndex, hit2D.point);
            currentBounceIndex++;

            //direction = Vector2.Reflect(direction, hit2D.normal);
            //position = hit2D.point + direction * 0.01f;

            if (reflectRemaining > 0 && currentBounceIndex <= maxSplitCount)
            {
                direction = Vector2.Reflect(direction, hit2D.normal);
                position = hit2D.point + direction * 0.01f;
                DrawLaserReflection(position, direction, --reflectRemaining);
            }


        }

    }
}

