using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using System;

public class AdmobRewardedInterstitialScript : GoogleAdmobAd
{
    public RewardedInterstitialAd rewardedInterstitial;
    public string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    public UnityEvent onAdLoadedCallback;
    public UnityEvent<Reward> onRewardAfterAd;

    private void Awake()
    {
        this.onAdLoadedCallback = new UnityEvent();
        this.onRewardAfterAd = new UnityEvent<Reward>();
    }

    public override void Start()
    {
        base.Start();
    }
    /// <summary>
    /// Llamar para requerir un anuncio, y despues llamar a ShowRewardedInterstitialAd
    /// </summary>
    public override void RequestAd()
    {
        base.RequestAd();
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        RewardedInterstitialAd.LoadAd(adUnitId, request, AdLoadCallback);

    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitial != null)
        {
            rewardedInterstitial.Show(UserEarnedRewardCallback);
        }
    }
    private void UserEarnedRewardCallback(Reward reward)
    {
        Debug.Log("ver que devuelve esto.  T:" + reward.Type + " , Amt: " + reward.Amount);
        onRewardAfterAd.Invoke(reward);
    }

    private void AdLoadCallback(RewardedInterstitialAd ad, string error) {
        if(error == null)
        {
            rewardedInterstitial = ad;
            Debug.Log("Rewarded interstitial finalizado");
            onAdLoadedCallback.Invoke();
        }
    }
    public override void HandleOnAdOpened(object sender, EventArgs args)
    {
        base.HandleOnAdOpened(sender, args);
    }
    public override void HandleOnAdLoaded(object sender, EventArgs args)
    {
        base.HandleOnAdLoaded(sender, args);
    }
    public override void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        base.HandleOnAdFailedToLoad(sender, args);
    }
    public override void HandleOnAdClosed(object sender, EventArgs args)
    {
        base.HandleOnAdClosed(sender, args);
    }
    public override void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        base.HandleOnAdLeavingApplication(sender, args);
    }
}
