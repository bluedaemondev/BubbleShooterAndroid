using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishGameOnTrigger : MonoBehaviour
{
    public string bubbleTag = "bubble";

    private void Start()
    {
        GameManagerActions.instance.defeatEvent.AddListener(DisplayInterstitialOnLose);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bubbleTag) && !collision.GetComponent<Bubble>().throwableMutated)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            GameManagerActions.instance.defeatEvent.Invoke();
        }
    }

    private void DisplayInterstitialOnLose()
    {
        AdmobComponentsManager.instance.RequestInterstitialAd();
    }
}
