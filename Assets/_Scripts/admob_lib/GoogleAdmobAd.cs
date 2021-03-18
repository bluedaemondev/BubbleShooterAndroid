using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdmobAd : MonoBehaviour
{
    public string componentTypeStringAdmob = ""; // strs de prueba en branch develop, strs produccion en main.

    public virtual void Start()
    {
        Debug.Log("Starting ad service");
    }

    

    /// <summary>
    /// Llamar para mostrar un anuncio
    /// </summary>
    public virtual void RequestAd()
    {
        Debug.Log("Loading ad...");
        AdmobComponentsManager.instance.onSendToTopAds.Invoke();

    }

    public virtual void RequestAd(AdPosition position)
    {
        Debug.Log("Loading ad... at " + position.ToString());
        //AdmobComponentsManager.instance.onSendToBackAds.Invoke();

    }

    public virtual void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public virtual void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public virtual void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");

    }

    public virtual void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public virtual void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
}
