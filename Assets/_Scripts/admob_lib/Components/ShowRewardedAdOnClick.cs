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

        foreach(var ctr in GameObject.FindObjectsOfType<Counter>())
        {
            FindObjectOfType<Gun>().BlockGun();
            ctr.StopTimer();
        }
        AdmobComponentsManager.instance.RequestRewardedAd();
        AdmobComponentsManager.instance.onSendToTopAds.Invoke();
    }

}
