using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class AdmobBannerScript : GoogleAdmobAd
{
    public string componentTypeStringAdmob = "ca-app-pub-3940256099942544/6300978111";
    private BannerView bannerView;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void RequestAd(AdPosition positionOnScreen = AdPosition.Bottom)
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(componentTypeStringAdmob, AdSize.Banner, positionOnScreen);
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

        base.RequestAd(positionOnScreen);
    }

    
}
