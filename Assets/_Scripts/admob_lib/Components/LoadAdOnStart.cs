using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdmobUnitType { 
    Banner,
    Interstitial,
    Rewarded_Interstitial
}

public class LoadAdOnStart : MonoBehaviour
{
    public AdmobUnitType toLoad = AdmobUnitType.Banner;

    // Start is called before the first frame update
    void Start()
    {
        switch (toLoad) {
            case AdmobUnitType.Banner:
                AdmobComponentsManager.instance.RequestBannerAd();
                break;
            case AdmobUnitType.Interstitial:
                AdmobComponentsManager.instance.RequestInterstitialAd();
                break;
            case AdmobUnitType.Rewarded_Interstitial:
                AdmobComponentsManager.instance.RequestRewardedAd();
                break;
        }

        AdmobComponentsManager.instance.onSendToTopAds.Invoke();
    }

}
