using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobInterstitialScript : GoogleAdmobAd
{
    public string appIdInterstitial = "a-app-pub-3940256099942544/1033173712"; // string instertitial
    public InterstitialAd interstitial;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }
    public override void RequestAd()
    {
        base.RequestAd();
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);


    }
    public override void HandleOnAdOpened(object sender, EventArgs args)
    {
        base.HandleOnAdOpened(sender, args);
        if (!GameManagerActions.instance.isPaused)
        {
            GameManagerActions.instance.onPause.Invoke();
        }
    }
    public override void HandleOnAdLoaded(object sender, EventArgs args)
    {
        base.HandleOnAdLoaded(sender, args);
        Debug.Log("Sender obj : " + sender.ToString());

    }
    public override void HandleOnAdClosed(object sender, EventArgs args)
    {
        base.HandleOnAdClosed(sender, args);

        GameManagerActions.instance.onResumeGame.Invoke();

    }
    public override void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        base.HandleOnAdFailedToLoad(sender, args);
        GameManagerActions.instance.onResumeGame.Invoke();
    }
    public override void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        base.HandleOnAdLeavingApplication(sender, args);
    }

}
