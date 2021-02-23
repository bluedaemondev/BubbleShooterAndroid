using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class AdmobBannerScript : GoogleAdmobAd
{
    private BannerView bannerView;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // test device str
        // cambiar por appIdBanner en entorno de produccion.
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    
}
