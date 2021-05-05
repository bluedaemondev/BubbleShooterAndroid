using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneLinkbuilder : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectOfType<AdmobBannerScript>() != null)
        {
            Destroy(FindObjectOfType<AdmobBannerScript>().gameObject);
            Destroy(FindObjectOfType<AdmobCanvasController>().gameObject);
        }
    }
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
    public void ReloadMainMenu()
    {
        ProgressSceneLoader.instance.LoadScene("MainMenu");
    }
}
