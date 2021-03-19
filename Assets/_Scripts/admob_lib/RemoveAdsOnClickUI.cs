using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class RemoveAdsOnClickUI : MonoBehaviour
{
    Button btnComponent;

    void Start()
    {
        btnComponent = this.GetComponent<Button>();
        btnComponent.onClick.AddListener(RemoveAds);
    }

    void RemoveAds()
    {
        foreach(var ad in FindObjectsOfType<AdmobCanvasController>())
        {
            Destroy(ad.gameObject);
        }
        
    }
}
