using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class ShowRewardedAdOnClick : MonoBehaviour
{
    UIManager _uiManager;

    [Header("Componente para UI Button")]
    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();

        this.btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ShowRewardedAd);
    }

    public void ShowRewardedAd()
    {

        foreach(var ctr in GameObject.FindObjectsOfType<Counter>())
        {
            FindObjectOfType<Gun>().BlockGun();
            ctr.StopTimer();
        }

        //AdmobComponentsManager.instance.RequestRewardedAd();
        AdmobComponentsManager.instance.RequestInterstitialAd();
        AdmobComponentsManager.instance.onSendToTopAds.Invoke();
    }

}
