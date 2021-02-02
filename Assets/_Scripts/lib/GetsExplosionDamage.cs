using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Bubble), typeof(Rigidbody2D))]
public class GetsExplosionDamage : MonoBehaviour
{
    Bubble bubbleScript;

    private void Awake()
    {
        //if (hScript == null)
        bubbleScript = this.GetComponent<Bubble>();

    }

    public void GetDamage()
    {
        bubbleScript.GetHit();
    }
}
