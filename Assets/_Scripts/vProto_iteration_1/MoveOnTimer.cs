using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTimer : MonoBehaviour
{
    [Header("Tiempo maximo")]
    public float timeMax;

    [Header("Velocidad"), Range(0.01f, 3f)]
    public float speedMov;

    [Header("Opcional para ir hasta una posicion de algo")]
    public GameObject transformMoveTowards;

    [Header("Opcional Direccion hacia donde se mueve")]
    public Vector2 direction;


    private Vector2 startPosition;
    private float timerCurrent;
    Coroutine tCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        this.startPosition = transform.position;
        tCoroutine = StartCoroutine(PassTime(timeMax));
    }

    IEnumerator PassTime(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("Exitting coroutine");


    }

    public void ResetToStart()
    {
        this.transform.position = startPosition;

    }

    public void MoveTowardsPosition()
    {
        Vector3 vecMov;
        if (this.transformMoveTowards != null)
        {
            vecMov = Vector3.MoveTowards(transform.position, transformMoveTowards.transform.position, Time.deltaTime * speedMov);
        }
        else
        {
            vecMov = transform.position - (Vector3)(Time.deltaTime * speedMov * direction);
        }

        transform.position = vecMov;

    }

    private void Update()
    {
        MoveTowardsPosition();
    }


}
