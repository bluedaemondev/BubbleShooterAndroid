using UnityEngine;

public class AdmobCanvasController : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        this.canvas = GetComponent<Canvas>();
        AdmobComponentsManager.instance.onSendToTopAds.AddListener(this.SendFront);
        AdmobComponentsManager.instance.onSendToBackAds.AddListener(this.SendBack);

    }
    public void SendBack()
    {
        this.canvas.sortingOrder = 0;
        Debug.Log("canvas : " + this.gameObject.name + " , sorting = " + this.canvas.sortingOrder);

    }
    public void SendFront()
    {
        this.canvas.sortingOrder = 11;
        Debug.Log("canvas : " + this.gameObject.name + " , sorting = " + this.canvas.sortingOrder);

    }
}
