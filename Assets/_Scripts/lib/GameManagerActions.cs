using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerActions : MonoBehaviour
{
    public static GameManagerActions instance { get; private set; }

    //[Header("Debugging, borrar")]
    //public KeyCode Restart = KeyCode.R;
    //public KeyCode Exit = KeyCode.Escape;

    public UnityEvent defeatEvent;
    public UnityEvent winEvent;
    public UnityEvent onPause;
    public UnityEvent onResumeGame;
    public UnityEvent onLoadGameScene;

    // transition = timeTransition:float , sceneNameToLoad:string
    //public UnityEvent<float, string> onTransition;

    public float gametime;


    public bool isPaused = false;


    private void Awake()
    {
        if (!instance)
            instance = this;

        if (defeatEvent == null)
            defeatEvent = new UnityEvent();
        if (winEvent == null)
            winEvent = new UnityEvent();
        if (onPause == null)
            onPause = new UnityEvent();
        if (onResumeGame == null)
            onResumeGame = new UnityEvent();
        //if (onTransition == null)
        //    onTransition = new UnityEvent<float, string>();

    }

    

    private void Start()
    {
        onPause.AddListener(PauseGame);
        onResumeGame.AddListener(ResumeGame);

        gametime = 0;
    }
    private void Update()
    {
        if (!isPaused)
            this.gametime += Time.deltaTime;
    }
    public void PauseGame()
    {
        Debug.Log("paused game");
        isPaused = true;
    }
    public void ResumeGame()
    {
        Debug.Log("resuming game");
        isPaused = false;

    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public AsyncOperation LoadSceneByNameAsync(string sceneName)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        return operation;
    }

    #region Deprecated
    //public void ReloadScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
    //public void LoadNextScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
    //public void LoadSceneByName(string name)
    //{
    //    SceneManager.LoadScene(SceneManager.GetSceneByName(name).buildIndex);
    //}
    #endregion
    public string GetTotalGametime()
    {
        var ts = TimeSpan.FromSeconds(gametime);
        return string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
    public bool CheckGameOver()
    {
        return false;
    }
}
