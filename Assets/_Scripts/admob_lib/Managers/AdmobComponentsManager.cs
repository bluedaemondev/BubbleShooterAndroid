using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;


/// To do : 
/// implementar un ad manager para controlar la inicializacion y tener las referencias
/// a estos objetos. DONE
/// Sacar el inicializador de la clase base y pasarlo al manager DONE
/// Implementar una interfaz para tener llamados posibles a la api de google admobs, usando estas llamadas
/// propias de tipo de anuncio. DONE
/// 

public class AdmobComponentsManager : MonoBehaviour
{
    public static AdmobComponentsManager instance { get; private set; }

    public UnityEvent onSendToBackAds;
    public UnityEvent onSendToTopAds;

    public string AppIdAdmob = "ca-app-pub-3940256099942544~3147511713"; // tests string, set to production on release

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onSendToTopAds = new UnityEvent();
        onSendToBackAds = new UnityEvent();

    }
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    public void RequestBannerAd()
    {
        var bannerController = new AdmobBannerScript();
        bannerController.RequestAd(AdPosition.Bottom);

    }
    public void RequestInterstitialAd()
    {
        var interstitialController = new AdmobInterstitialScript();
        interstitialController.RequestAd();
    }
    public void RequestRewardedAd()
    {
        var rewardedInterstitialController = new AdmobRewardedInterstitialScript();
        rewardedInterstitialController.RequestAd();
    }



}
