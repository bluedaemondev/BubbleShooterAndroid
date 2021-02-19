using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject howToPlayPanel;

    public string redSocialLink1 = "http://instagram.com/bluedaemonart";


    private void Awake()
    {
        ShowMainMenu();
    }

    public void InstagramLink()
    {
        Application.OpenURL(redSocialLink1);
    }
    public void YoutubeLink()
    {
        Application.OpenURL("https://www.youtube.com/channel/UC6yn5SLH7tFb5_-hEPkfx3Q");
    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);

        //creditsPanel.SetActive(false);
        //howToPlayPanel.SetActive(false);
    }
    public void PlayNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //ManagerIntroduccion.instance.startIntroductionEvent.Invoke();
        //ManagerIntroduccion.instance.DisableChildren();

        Debug.Log("playing ");
        //this.mainPanel.SetActive(false);
        //this.howToPlayPanel.SetActive(false);
        //this.creditsPanel.SetActive(false);

    }
    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        //print("credits");
        mainPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }
    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        //print("controles");

        mainPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}