using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickSystem : MonoBehaviour
{
    public static TickSystem instance { get; private set; }

    public const int ticksToAction = 5;

    public int currentTicks = 0;
    public UnityEvent onTurnPassed;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);

        onTurnPassed = new UnityEvent();
    }

    public void Tick()
    {
        if (currentTicks < ticksToAction)
            this.currentTicks++;
        else
        {
            this.currentTicks = 0;
            this.onTurnPassed.Invoke();
        }
    }

    //private void Update()
    //{
        
    //}

}
