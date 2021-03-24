/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public BallManager ballManager;
    public UIManager uiManager;
    public InputController inputController;

    public Gun gun;
    public Pusher pusher;
    public Deadline deadline;

    public LevelProfile _level;
    public LevelProfile GetLevelProfile(){
        return _level;
    }

    Common.GameState _gameState;

    // Use this for initialization
    void Start()
    {
        gun.InitGun(this);
        pusher.InitPusher(this);
        deadline.InitLine(this);

        registerEventScore();
        registerEventTouch();
        registerEventWin();
        OnStartGame();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    #region Game state
    public void OnStartGame()
    {
        _gameState = Common.GameState.Playing;

        gun.LoadDoneBullets(ballManager.GenerateBallAsBullet(), ballManager.GenerateBallAsBullet());
        gun.UnBlockGun();

        pusher.OnResume();

        ballManager.InitBalls(GetLevelProfile());
        uiManager.NormalMode();

        AudioManager.Instance.PlaySound(AudioManager.Instance.click);
    }

    public void OnShootAction()
    {
        Ball newBullet = ballManager.GenerateBallAsBullet();
        gun.LoadBullets(newBullet);
        AudioManager.Instance.PlaySound(AudioManager.Instance.shoot);
    }

    public float OnPushDown()
    {
        float heightDown = ballManager.PushDown();
        return heightDown;
    }

    public void OnGameover()
    {
//        Debug.Log("Gameover");
        _gameState = Common.GameState.Gameover;

        pusher.OnPause();

        gun.BlockGun();

        uiManager.DisplayGameOver();

        AudioManager.Instance.PlaySound(AudioManager.Instance.gameover);
        AudioManager.Instance.PlayThemeMenu();

    }

    public void OnReset()
    {
        ballManager.ClearBalls();
        ballManager.Reset();

        gun.ClearBullets();
        gun.ResetGunDirection();

        pusher.Reset();

        deadline.Reset();

        uiManager.DisableText();
        uiManager.UpdateScore(0);

        AudioManager.Instance.PlayThemeGame();

    }

    public void OnWin()
    {
        _gameState = Common.GameState.Gameover;

        pusher.OnPause();

        gun.BlockGun();

        uiManager.DisplayWin();

        AudioManager.Instance.PlaySound(AudioManager.Instance.win);

    }

    public void OnWarning()
    {
        uiManager.TurnOnRedAlert();
        AudioManager.Instance.PlaySound(AudioManager.Instance.warning);

    }
    #endregion 

    #region UI
    void displayScore(int score)
    {
        uiManager.UpdateScore(score);
    }
    #endregion 

    #region Events

    void registerEventTouch()
    {
        inputController.RegisterEventTouch((Vector3 position) =>
            {
                if (_gameState == Common.GameState.Gameover)
                {
                    OnReset();
                    OnStartGame();
                }
            });
    }

    void registerEventWin()
    {
        ballManager.RegisterEventClearBall(OnWin);
    }

    void registerEventScore()
    {
        ballManager.RegisterEventCalculateScore(displayScore);
    }

    #endregion


}
