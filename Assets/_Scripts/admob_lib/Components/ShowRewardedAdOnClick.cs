using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class ShowRewardedAdOnClick : MonoBehaviour
{
    [Header("Componente para UI Button")]
    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ShowRewardedAd);
    }

    public void ShowRewardedAd()
    {
        var rewardedIntstInstance = FindObjectOfType<AdmobRewardedInterstitialScript>();
        rewardedIntstInstance.RequestAd();

        rewardedIntstInstance.ShowRewardedInterstitialAd();

        AdmobComponentsManager.instance.onSendToTopAds.Invoke();
    }

}
