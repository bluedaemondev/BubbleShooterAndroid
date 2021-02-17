using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerActions : MonoBehaviour
{
    public static GameManagerActions instance { get; private set; }

    [Header("Debugging, borrar")]
    public KeyCode Restart = KeyCode.R;
    public KeyCode Exit = KeyCode.Escape;

    public UnityEvent defeatEvent;
    public UnityEvent winEvent;
    public UnityEvent onPause;
    public UnityEvent onResumeGame;


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
    }
    private void Start()
    {
        onPause.AddListener(PauseGame);
        onResumeGame.AddListener(ResumeGame);


    }
    public void PauseGame()
    {
        Debug.Log("paused game");
        isPaused = true;
        
        //if (Time.timeScale == 0)
        //{
        //    Time.timeScale = 1;
        //}
        //else
        //{
        //    Time.timeScale = 0;
        //}
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
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
