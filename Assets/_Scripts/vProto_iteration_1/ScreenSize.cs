using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    [Header("Camara desde donde escalo el objeto.")]
    public Transform scaleCamera;



    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Utils.instance.MainCam.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }
    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            //Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

            Vector2 edgeVector = Utils.instance.MainCam.ViewportToWorldPoint(topRightCorner);
            
            var width = edgeVector.x * 2;
            return width;
        }
    }
}
