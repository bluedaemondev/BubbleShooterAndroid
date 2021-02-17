using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public float forceMagnitudeShoot = 10f;
    [Header("Cantidad de burbujas disponibles")]
    public int throwableCount = 2;

    Queue<Bubble> currentThrowables;

    [Space]
    [Header("Posiciones en donde aparecen las burbujas disparables")]
    public GameObject spawnPrimaryBubble;
    public GameObject spawnSecondaryBubble;

    private bool canShoot = true;

    public void FeedBubbleShooter()
    {
        //for (int i = 0; i < throwableCount - currentThrowables.Count; i++) //0 - 2
        int i = 0;
        while (throwableCount != currentThrowables.Count)
        {
            Bubble bubble = Instantiate(ObjectPooler.instance.pools[0].prefab).GetComponent<Bubble>();
            bubble.GenerateThrowableBubble();
            currentThrowables.Enqueue(bubble);

            bubble.transform.parent = i == 0 ? spawnPrimaryBubble.transform : spawnSecondaryBubble.transform;
            bubble.transform.position = i == 0 ? spawnPrimaryBubble.transform.position : spawnSecondaryBubble.transform.position;
            i++;
        }
    }

    void SwitchBubblePriority()
    {
        var aux = spawnPrimaryBubble;
        spawnPrimaryBubble = spawnSecondaryBubble;
        spawnSecondaryBubble = aux;


        var auxQ = currentThrowables.Dequeue();
        var auxQ2 = currentThrowables.Dequeue();
        Debug.Log("first dequeue = " + auxQ.name);
        Debug.Log("second dequeue = " + auxQ2.name);


        currentThrowables.Enqueue(auxQ2);
        currentThrowables.Enqueue(auxQ);

        Debug.Log("first enqueue = " + auxQ2.name);
        Debug.Log("second enqueue = " + auxQ.name);

    }


    public void OnBubbleThrow()
    {
        var thrown = currentThrowables.Dequeue();

        thrown.transform.parent = null;

        var force = (Utils.instance.MouseToWorldWithoutZ() - transform.position).normalized;
        force *= forceMagnitudeShoot;

        thrown.SetImpulseForce(force);

        FeedBubbleShooter();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (currentThrowables == null)
        {
            currentThrowables = new Queue<Bubble>();
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
    // Update is called once per frame
    void Update()
    {
        //canShoot = Vector2.Distance(Utils.instance.MouseToWorldWithoutZ(), transform.position) >= 1.2f;

        if (Input.GetMouseButtonUp(0) && canShoot && !GameManagerActions.instance.isPaused)
        {
            OnBubbleThrow();
        }
    }

}
