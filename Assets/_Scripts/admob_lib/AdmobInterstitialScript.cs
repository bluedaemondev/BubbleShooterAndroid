using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobInterstitialScript : GoogleAdmobAd
{
    public new string appId = "";
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
    }
    public override void HandleOnAdLoaded(object sender, EventArgs args)
    {
        base.HandleOnAdLoaded(sender, args);
    }
    public override void HandleOnAdClosed(object sender, EventArgs args)
    {
        base.HandleOnAdClosed(sender, args);
    }
    public override void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        base.HandleOnAdFailedToLoad(sender, args);
    }
    public override void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        base.HandleOnAdLeavingApplication(sender, args);
    }

}
