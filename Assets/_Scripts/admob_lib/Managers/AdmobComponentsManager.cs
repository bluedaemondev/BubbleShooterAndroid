using UnityEngine;
using UnityEngine.Events;

public class AdmobComponentsManager : MonoBehaviour
{
    public static AdmobComponentsManager instance { get; private set; }

    public UnityEvent onSendToBackAds;
    public UnityEvent onSendToTopAds;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onSendToTopAds = new UnityEvent();
        onSendToBackAds = new UnityEvent();

    }
    


}
