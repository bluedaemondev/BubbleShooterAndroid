using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject howToPlayPanel;

    public string sceneLoadOnPlay = "ComicIntroSlides";
    public string redSocialLink1 = "https://lordfibonacci.bandcamp.com/";
    public string redSocialLink2 = "https://lordfibonacci.bandcamp.com/track/judgment-break-drum-break";




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
        Application.OpenURL(redSocialLink2);
    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);

        //creditsPanel.SetActive(false);
        //howToPlayPanel.SetActive(false);
    }
    public void PlayNextScene()
    {
        //FindObjectOfType<ProgressSceneLoader>().LoadScene(sceneLoadOnPlay);
        ProgressSceneLoader.instance.LoadScene(sceneLoadOnPlay);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MainMenu"));
        
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