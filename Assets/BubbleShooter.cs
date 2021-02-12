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


    public void FeedBubbleShooter()
    {
        for (int i = 0; i < throwableCount - currentThrowables.Count; i++)
        {
            Bubble bubble = new Bubble();
            bubble.GenerateThrowableBubble();
            currentThrowables.Enqueue(bubble);
        }
    }

    public void OnBubbleThrow()
    {
        var thrown = currentThrowables.Dequeue();
        var force = (Utils.instance.MouseToWorldWithoutZ() - transform.position).normalized;
        force *= forceMagnitudeShoot;

        thrown.SetImpulseForce(force);

        //thrown.GetComponent<Rigidbody2D>().AddForce()
        FeedBubbleShooter();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (currentThrowables == null)
        {
            currentThrowables = new Queue<Bubble>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
