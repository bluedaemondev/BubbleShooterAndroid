using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobInterstitialScript : GoogleAdmobAd
{
    public new string appIdInterstitial = "a-app-pub-3940256099942544/1033173712"; // string instertitial
    public InterstitialAd interstitial;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void RequestAd()
    {

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(componentTypeStringAdmob);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
        this.interstitial.Show();

        base.RequestAd();


    }
    public override void HandleOnAdOpened(object sender, EventArgs args)
    {
        base.HandleOnAdOpened(sender, args);
        if (!GameManagerActions.instance.isPaused)
        {
            GameManagerActions.instance.onPause.Invoke();
            //AdmobComponentsManager.instance.onSendToTopAds.Invoke();
        }

        this.interstitial.OnAdOpening -= HandleOnAdOpened;

    }
    public override void HandleOnAdLoaded(object sender, EventArgs args)
    {
        base.HandleOnAdLoaded(sender, args);
        Debug.Log("Sender obj : " + sender.ToString());


        this.interstitial.OnAdLoaded -= HandleOnAdLoaded;


    }
    public override void HandleOnAdClosed(object sender, EventArgs args)
    {
        base.HandleOnAdClosed(sender, args);

        GameManagerActions.instance.StartCoroutine(GameManagerActions.instance.DelayedResume());
        this.interstitial.OnAdClosed -= HandleOnAdClosed;

        AdmobComponentsManager.instance.onSendToBackAds.Invoke();

    }
    public override void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        base.HandleOnAdFailedToLoad(sender, args);
        GameManagerActions.instance.StartCoroutine(GameManagerActions.instance.DelayedResume());

        this.interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;

    }
    public override void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        base.HandleOnAdLeavingApplication(sender, args);
        this.interstitial.OnAdLeavingApplication -= HandleOnAdLeavingApplication;

    }


}
