/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text _centerText;
    public List<GameObject> _gameoverUtils;
    public List<GameObject> _winUtils;

    public List<GameObject> animatedCanvas;
    public GameObject pauseDrawer;

    public Text _score;
    public Parallax _background;
    public GameManager gamemanager;

    public void OnPause()
    {
        //var counters = FindObjectsOfType<Counter>();
        //foreach (var _counter in counters)
        //{
        //    if (_counter.CurrentState != Counter.CounterState.STOP)
        //        _counter.StopTimer();
        //    else
        //        _counter.ContinueTimer();
        //}

        if (!gamemanager.gun.GetBlockedState())
        {
            Debug.Log("blocked not");
            //gamemanager.gun.shoot(Vector3.back);
            //gamemanager.gun.Controller.UnRegisterEventTouch(gamemanager.gun.shoot);
            gamemanager.gun.BlockGun();
            //GameObject.FindObjectOfType<Gun>().CheckIntegrity();
        }
        else
        {
            Debug.Log("blocked not");

            gamemanager.gun.UnBlockGun();
            gamemanager.gun.ClearBullets();

            gamemanager.gun.LoadDoneBullets(gamemanager.ballManager.GenerateBallAsBullet(), gamemanager.ballManager.GenerateBallAsBullet());
            //gamemanager.gun.Controller.RegisterEventTouch(gamemanager.gun.shoot);
        }
    }

    public void OnStartNewGame()
    {
        foreach (var item in _gameoverUtils)
            item.SetActive(false);
        foreach (var item in animatedCanvas)
            item.SetActive(true);
    }
    public void DisplayGameOver()
    {
        _centerText.gameObject.SetActive(true);

        foreach (var item in _gameoverUtils)
            item.SetActive(true);
        foreach (var item in animatedCanvas)
            item.SetActive(false);

        //_centerText.text = "Game Over";

        if (_background)
            _background.StopMode();
    }

    public void DisplayWin()
    {
        _centerText.gameObject.SetActive(true);
        //_centerText.text = "Thanks for playing!";

        foreach (var item in _winUtils)
            item.SetActive(true);
        foreach (var item in animatedCanvas)
            item.SetActive(false);

        if (_background)
            _background.StopMode();
    }

    public void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }

    public void DisableText()
    {
        _centerText.gameObject.SetActive(false);
    }

    public void TurnOnRedAlert()
    {
        if (_background)
        {
            _background.RedAlert();
        }
    }

    public void NormalMode()
    {
        if (_background)
            _background.NormalMode();
    }

}
