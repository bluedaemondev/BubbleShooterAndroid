using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils instance { get; private set; }

    private Camera _mainCam;

    public Camera MainCam
    {
        get { return _mainCam; }
        set { SetMainCamera(value); }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            instance.MainCam = Camera.main;
        }
    }
    public bool GetTouchEnding()
    {
        bool res = false;
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            res = Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        res = Input.GetMouseButtonUp(0);
#endif
        return res;
    }

    public void SetMainCamera(Camera mainCamScene)
    {
        instance._mainCam = mainCamScene;
    }

    public bool GetContinuousTouch()
    {
        bool res = false;
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            res = Input.GetTouch(0).phase == TouchPhase.Moved;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        res = Input.GetMouseButton(0);
#endif
        //Debug.Log("se llama " + res);
        return res;
    }
    public bool GetInitialTouch()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        else
            return false;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#endif
    }
    public bool GetStationaryTouch()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Input.GetTouch(0).phase == TouchPhase.Stationary;
#endif
    }
    public Vector3 MouseToWorldWithoutZ()
    {
        Vector3 result = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        result.z = 0;
        return result;
    }
}
