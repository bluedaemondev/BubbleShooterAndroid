using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrentBubbleSwitch : MonoBehaviour
{

    public static CurrentBubbleSwitch instance { get; private set; }
    public UnityEvent onSwitchBubble;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onSwitchBubble = new UnityEvent();
        onSwitchBubble.AddListener(SwitchPositions);
    }
    void SwitchPositions()
    {
        //StartCoroutine()
        //int throwableContainers = transform.GetChild(0).childCount;
        //var baseChild = transform.GetChild(0);
        //for (int i=0; i < throwableContainers; i++)
        //{
        //    Debug.Log("chld " + baseChild.GetChild(i).name);

        //}
    }
    // Update is called once per frame
    void OnMouseDown()
    {
        Debug.Log("Switching to new bubble");

        onSwitchBubble.Invoke();
        // - get animator
        // - play switch animation
        // - make the switch via code
        
    }

}
