using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootFromPoint : MonoBehaviour
{
    [Header("Disparo desde y base bubble prefab")]
    public GameObject gunpoint;
    public GameObject prefabGenericBubble; // se construye sola con un color en base a lo que hay en el mapa

    [Space]
    [Header("Totales del jugador")]
    public int currBubblesAmount = 40;
    public int maxBubblesAmount = 40;
    public float gunCooldown = 0.25f;
    public float currentCooldown = 0;

    [Space]
    [Header("Totales del jugador")]
    public float maxAngleAim = 75;

    [Space]
    [Header("Audios a utilizar")]
    public AudioClip clipShootStandard;

    SpriteRenderer gunHolder;
    private void Start()
    {
        GameManagerActions.instance.defeatEvent.AddListener(DisableComponent);
    }
    public void DisableComponent()
    {
        this.enabled = false;
    }

    private void Update()
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;

            TurnGunpoint(mousePos);
        }
        
    }
    private Vector3 TurnGunpoint(Vector3 mousePos)
    {
        Vector3 aimDirection = (mousePos - transform.position).normalized;
        float angleRot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if(Mathf.Abs(angleRot) > maxAngleAim)
        {
            var auxRot = Mathf.Clamp(angleRot, -maxAngleAim, maxAngleAim);
            Debug.Log("clamping " + angleRot + " , to " + auxRot);
        }

        gunpoint.transform.eulerAngles = new Vector3(0, 0, angleRot);

        return gunpoint.transform.eulerAngles;
    }

    /// <summary>
    /// Terminar de corregir cuando este la clase bubble
    /// </summary>
    /// <param name="mousePos"></param>
    void ShootBullet(Vector3 mousePos)
    {
        var bubble_go = Instantiate(prefabGenericBubble, gunpoint.transform.position, Quaternion.identity);
        //var bubble = bubble_go.GetComponent<Bubble>();

        var directionVector = (mousePos - gunpoint.transform.position).normalized;

        //bubble.forceToAppend = directionVector;
        //bubble.parentToIgnoreCol = this.GetComponent<Collider2D>();


        this.currentCooldown = 0; // Reseteo el cd

    }

}
