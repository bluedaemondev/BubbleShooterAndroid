using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using System;

public class AdmobRewardedInterstitialScript : GoogleAdmobAd
{
    public RewardedInterstitialAd rewardedInterstitial;
    public new string componentTypeStringAdmob = "ca-app-pub-3940256099942544/1033173712";

    public UnityEvent onAdLoadedCallback;
    public UnityEvent<Reward> onRewardAfterAd;

    //private void Awake()
    //{
    //}

    public override void Start()
    {
        base.Start();
        this.onAdLoadedCallback = new UnityEvent();
        this.onRewardAfterAd = new UnityEvent<Reward>();

    }


    public override void RequestAd()
    {

        AdRequest request = new AdRequest.Builder().Build();
        RewardedInterstitialAd.LoadAd(componentTypeStringAdmob, request, AdLoadCallback);
        ShowRewardedInterstitialAd();

        base.RequestAd();
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
            //AdmobComponentsManager.instance.onSendToTopAds.Invoke();
            //StartCoroutine(SendAdToTopAfterPresentation());


        }
    }

    private void UserEarnedRewardCallback(Reward reward)
    {
        Debug.Log("ver que devuelve esto.  T:" + reward.Type + " , Amt: " + reward.Amount);
        //onRewardAfterAd.Invoke(reward);
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
            //onAdLoadedCallback.Invoke();
        }
    }
   
    public void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        print("Rewarded interstitial ad has received a paid event.");
        this.rewardedInterstitial.OnPaidEvent -= HandlePaidEvent;

        //to do: representacion visual de recompensa

        if (GameManagerActions.instance.isPaused)
        {
            StartCoroutine(base.DelayedResume());
        }
        //base.HandleOnAdOpened(sender, args);
    }
    public void HandleAdDidDismiss(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has dismissed presentation.");
        this.rewardedInterstitial.OnAdDidDismissFullScreenContent -= HandleAdDidDismiss;
        if (GameManagerActions.instance.isPaused)
        {
            //StartCoroutine(base.DelayedResume());
            GameManagerActions.instance.onResumeGame.Invoke();
        }


        //base.HandleOnAdLoaded(sender, args);
    }
    public void HandleAdDidPresent(object sender, EventArgs args)
    {
        print("Rewarded interstitial ad has presented.");
        //base.HandleOnAdFailedToLoad(sender, args);
        this.rewardedInterstitial.OnAdDidPresentFullScreenContent -= HandleAdDidPresent;

        //AdmobComponentsManager.instance.onSendToTopAds.Invoke();
    }

    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Rewarded interstitial ad has failed to present.");
        this.rewardedInterstitial.OnAdFailedToPresentFullScreenContent -= HandleAdFailedToPresent;
        if (GameManagerActions.instance.isPaused)
        {
            StartCoroutine(base.DelayedResume());
        }
    }


}
