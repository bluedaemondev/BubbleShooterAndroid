/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    Vector2 touchOrigin;
    Counter counter;

    public delegate void TouchEvent(Vector3 touchPosition);
    public delegate void DragEvent(Vector3 touchPosition);
    public TouchEvent touchEvent;
    public DragEvent dragEvent;

    // Use this for initialization
    void Start()
    {
        counter = GetComponent<Counter>();
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        onReceiveController();
    }

    void onReceiveController()
    {
        #if UNITY_EDITOR 
        if(Input.mousePresent){
            if(Input.GetMouseButtonDown(0)){
                if (counter.CurrentState == Counter.CounterState.STOP)
                {
                    counter.StartTimerUpdateSeconds(0.1f, null, null);     
                    OnTouch(Input.mousePosition);
                }
            } else {
                if (counter.CurrentState == Counter.CounterState.STOP)
                {
                    OnDrag(Input.mousePosition);  
                }
            }
        }
       
        #else 
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Moved)
            {
                if (counter.CurrentState == Counter.CounterState.STOP)
                {
                     OnDrag(new Vector3(myTouch.position.x, myTouch.position.y, 0));
                }
            }
            else if (myTouch.phase == TouchPhase.Ended)
            {

                Vector2 touchEnd = myTouch.position;
                if(Vector2.Distance(touchEnd, touchOrigin) < 5f){
                    if (counter.CurrentState == Counter.CounterState.STOP)
                    {
                        counter.StartTimerUpdateSeconds(0.1f, null, null);    
                        OnTouch(new Vector3(touchEnd.x, touchEnd.y, 0));
                    }
                }
            }
        }
        #endif
    }

    public void OnTouch(Vector3 position)
    {
        //Debug.Log("Touch " + position);
        if (touchEvent != null)
        {
            touchEvent(position);
        }
    }

    public void OnDrag(Vector3 position)
    {
        //Debug.Log("Drag " + position);
        if (dragEvent != null)
        {
            dragEvent(position);
        }
    }

    #region Register event
    public void RegisterEventTouch(TouchEvent teventFunc){
        touchEvent += teventFunc;
    }

    public void RegisterEventDrag(DragEvent deventFunc){
        dragEvent += deventFunc;
    }

    public void UnRegisterEventTouch(TouchEvent teventFunc){
        touchEvent -= teventFunc;
    }

    public void UnRegisterEventDrag(DragEvent deventFunc){
        dragEvent -= deventFunc;
    }
    #endregion 
}
