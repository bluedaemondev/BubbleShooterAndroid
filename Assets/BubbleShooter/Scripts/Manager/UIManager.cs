/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text _centerText;
    public GameObject _gameoverUtils;

    public Text _score;
    public Parallax _background;

    // Use this for initialization
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void DisplayGameOver()
    {
        _centerText.gameObject.SetActive(true);
        //_gameoverUtils.SetActive(true);

        _centerText.text = "Game Over";

        if (_background)
            _background.StopMode();
    }

    public void DisplayWin()
    {
        _centerText.gameObject.SetActive(true);
        _centerText.text = "Win";
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
