using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    [Header("Propiedades")]
    [InspectorName("Fuerza de tiro")]
    public float forceMagnitudeShoot = 10f;
    [InspectorName("Cooldown entre disparos")]
    public float cooldownBubbleShoot = 0.3f;

    [Header("Cantidad de burbujas disponibles")]
    public int throwableCount = 2;

    public Queue<ThrownBubble> currentThrowables;

    [Space]
    [Header("Posiciones en donde aparecen las burbujas disparables")]
    public GameObject spawnPrimaryBubble;
    public GameObject spawnSecondaryBubble;

    

    private bool canShoot = true;

    public void FeedBubbleShooter()
    {
        //for (int i = 0; i < throwableCount - currentThrowables.Count; i++) //0 - 2
        int i = 0;
        while (throwableCount > currentThrowables.Count)
        {
            ThrownBubble bubble = ObjectPooler.instance.SpawnFromPool("thrownBubble").GetComponent<ThrownBubble>();//Instantiate(ObjectPooler.instance.pools[1].prefab).GetComponent<ThrownBubble>();
            
            currentThrowables.Enqueue(bubble);

            bubble.transform.parent = spawnPrimaryBubble.transform.childCount == 0 ? spawnPrimaryBubble.transform : spawnSecondaryBubble.transform;
            //bubble.transform.position = i == 0 ? spawnPrimaryBubble.transform.position : spawnSecondaryBubble.transform.position;
            bubble.transform.localPosition = Vector3.zero;

            i++;
        }
    }

    /// <summary>
    // Switch current bubble 2 -> 1 
    /// </summary>
    public void SwitchBubblePriority()
    {

        var auxBubbleSec = currentThrowables.Dequeue(); // first in

        auxBubbleSec.transform.parent = spawnPrimaryBubble.transform;
        auxBubbleSec.transform.localPosition = Vector3.zero;

        currentThrowables.Enqueue(auxBubbleSec);

        if (currentThrowables.Count <= 1)
            FeedBubbleShooter();
        
    }


    public void OnBubbleThrow()
    {
        var thrown = currentThrowables.Dequeue();
        thrown.transform.parent = null;


        var force = (Utils.instance.MouseToWorldWithoutZ() - transform.position).normalized;
        force *= forceMagnitudeShoot;

        thrown.SetImpulseForce(force);

        TickSystem.instance.Tick();
        CurrentBubbleSwitch.instance.onSwitchBubble.Invoke();
        StartCoroutine(ResumeAfterTime(cooldownBubbleShoot));
    }


    // Start is called before the first frame update
    void Start()
    {
        if (currentThrowables == null)
        {
            currentThrowables = new Queue<ThrownBubble>();
        }

        FeedBubbleShooter();

        CurrentBubbleSwitch.instance.onSwitchBubble.AddListener(SwitchBubblePriority);
        GameManagerActions.instance.onPause.AddListener(DisableControl);
        GameManagerActions.instance.onResumeGame.AddListener(EnableControl);


    }

    public void DisableControl()
    {
        this.canShoot = false;
    }
    public void EnableControl()
    {
        StartCoroutine(ResumeAfterTime(1f));
    }

    private IEnumerator ResumeAfterTime(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        canShoot = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && canShoot && !GameManagerActions.instance.isPaused)
        {
            OnBubbleThrow();
        }
    }

}
