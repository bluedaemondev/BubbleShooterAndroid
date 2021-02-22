using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishGameOnTrigger : MonoBehaviour
{
    public string bubbleTag = "bubble";
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bubbleTag))
        {
            GameManagerActions.instance.defeatEvent.Invoke();
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
