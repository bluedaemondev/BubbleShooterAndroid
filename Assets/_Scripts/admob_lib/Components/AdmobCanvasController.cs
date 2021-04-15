using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdmobCanvasController : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!canvas)
            this.canvas = GetComponent<Canvas>();
    }

    void OnEnable()
    {
        AdmobComponentsManager.instance.onSendToTopAds.AddListener(this.SendFront);
        AdmobComponentsManager.instance.onSendToBackAds.AddListener(this.SendBack);

    }
    public void SendBack()
    {
        this.canvas.sortingOrder = 0;
        Debug.Log("SENDBACK canvas : " + this.gameObject.name + " , sorting = " + this.canvas.sortingOrder);

    }
    private void SendFront()
    {
        Debug.Log("canvas : " + this.gameObject.name + " , sorting = " + this.canvas.sortingOrder);
        //yield return new WaitForSeconds(0.2f);
        this.canvas.sortingOrder = 11;
        Debug.Log("SENDFRONT canvas : " + this.gameObject.name + " , sorting = " + this.canvas.sortingOrder);

    }
}
