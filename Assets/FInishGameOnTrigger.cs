using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishGameOnTrigger : MonoBehaviour
{
    public string tagPlayer = "Player";
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagPlayer))
        {
            GameManagerActions.instance.winEvent.Invoke();
            this.GetComponent<BoxCollider2D>().enabled = false;
            //Destroy(this.gameObject);
        }
    }
}
