﻿using System.Collections;
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

            if (!GameManagerActions.instance.isPaused)
            {
                GameManagerActions.instance.onPause.Invoke();
            }
        }
    }
    private void UserEarnedRewardCallback(Reward reward)
    {
        Debug.Log("ver que devuelve esto.  T:" + reward.Type + " , Amt: " + reward.Amount);
        onRewardAfterAd.Invoke(reward);
    }

    private void AdLoadCallback(RewardedInterstitialAd ad, string error)
    {
        if (error == null)
        {
            rewardedInterstitial = ad;

            rewardedInterstitial.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
            rewardedInterstitial.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
            rewardedInterstitial.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
            rewardedInterstitial.OnPaidEvent += HandlePaidEvent;

            Debug.Log("Rewarded interstitial finalizado");
            onAdLoadedCallback.Invoke();
        }
    }
    public void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        print("Rewarded interstitial ad has received a paid event.");
        this.rewardedInterstitial.OnPaidEvent -= HandlePaidEvent;

        //to do: representacion visual de recompensa

        if (GameManagerActions.instance.isPaused)
        {
            StartCoroutine(DelayedResume());
        }
        //base.HandleOnAdOpened(sender, args);
    }
    public void HandleAdDidDismiss(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has dismissed presentation.");
        this.rewardedInterstitial.OnAdDidDismissFullScreenContent -= HandleAdDidDismiss;
        if (GameManagerActions.instance.isPaused)
        {
            StartCoroutine(DelayedResume());
        }


        //base.HandleOnAdLoaded(sender, args);
    }
    public void HandleAdDidPresent(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has presented.");
        //base.HandleOnAdFailedToLoad(sender, args);
        this.rewardedInterstitial.OnAdDidPresentFullScreenContent -= HandleAdDidPresent;

    }

    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Rewarded interstitial ad has failed to present.");
        this.rewardedInterstitial.OnAdFailedToPresentFullScreenContent -= HandleAdFailedToPresent;
        if (GameManagerActions.instance.isPaused)
        {
            StartCoroutine(DelayedResume());
        }
    }


}
