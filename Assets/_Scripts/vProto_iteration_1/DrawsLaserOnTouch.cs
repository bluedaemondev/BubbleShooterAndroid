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
    bool hasChangedTouch;

    int currentBounceIndex = 0;



    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        SetStartingLaserPoint();
    }

    void SetStartingLaserPoint()
    {

        lineRend.positionCount = 1;
        lineRend.SetPosition(0, transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        if (Utils.instance.GetContinuousTouch())
        {
            var dirMouse = (transform.position - Utils.instance.MouseToWorldWithoutZ()).normalized * maxDist;
            


            DrawLaserReflection(transform.position, dirMouse, maxSplitCount);
        }
        else if (Utils.instance.GetTouchEnding())
        {
            SetStartingLaserPoint();
            currentBounceIndex = 2;

        }
        else if (Utils.instance.GetInitialTouch())
        {
            var dirMouse = (transform.position - Utils.instance.MouseToWorldWithoutZ()).normalized * maxDist;
            currentBounceIndex = lineRend.positionCount = 2;
            lineRend.SetPosition(1, transform.position + dirMouse);
            print("chupame bien la verga unity de mierda");
        }
    }

    void DrawLaserReflection(Vector2 position, Vector2 direction, int reflectRemaining)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, direction, maxDist);

        if (hit2D && currentBounceIndex <= maxSplitCount)
        {
            lineRend.positionCount = currentBounceIndex + 1;
            Debug.Log(currentBounceIndex);
            lineRend.SetPosition(currentBounceIndex, hit2D.point);
            currentBounceIndex++;

            //direction = Vector2.Reflect(direction, hit2D.normal);
            //position = hit2D.point + direction * 0.01f;

            if (reflectRemaining > 0)
            {
                direction = Vector2.Reflect(direction, hit2D.normal);
                position = hit2D.point + direction * 0.01f;
                DrawLaserReflection(position, direction, --reflectRemaining);
            }
            
        }
        
    }
}

