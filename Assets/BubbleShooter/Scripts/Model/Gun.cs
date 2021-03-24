/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{

    public InputController Controller;
    public float Force;
    public Transform BulletTransform;
    public Transform PreBulletTransform;
    public Transform BulletsRoot;

    Ball _bullet;
    Ball _preBullet;

    private GameManager _gameManager;
    private Counter _counter;
    private bool _isGunReady;
    private bool _isBlock;

    // Use this for initialization
    void Start()
    {
        registerEvents();
        _counter = GetComponent<Counter>();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public void InitGun(GameManager gameManager)
    {
        _gameManager = gameManager;   
    }

    public void ClearBullets()
    {
        Destroy(_bullet.gameObject);
        Destroy(_preBullet.gameObject);
    }

    public void ResetGunDirection(){
        transform.up = Vector3.up;
    }

    void registerEvents()
    {
        Controller.RegisterEventTouch(shoot);
        Controller.RegisterEventDrag(rotateGun);
    }

    void shoot(Vector3 position)
    {
        if (_isGunReady && !_isBlock)
        {
            
            Vector3 direction = transform.up;
            Vector3 force = direction.normalized * Force * 1000;
            //Debug.Log("Shoot ball color " +  _bullet.GetBallColor() + " with force " + force);
            _bullet.WasShoot(BulletsRoot, force);
            _isGunReady = false;
            _gameManager.OnShootAction();
        }
    }

    void rotateGun(Vector3 position)
    {
        if (!_isBlock)
        {
            Vector3 direction = position - transform.position;
            //Debug.Log(Vector3.Angle(transform.up, direction));
            if (Vector3.Angle(Vector3.up, direction) < 60)
            {
                transform.up = direction;
            }
        }
    }

    public void LoadBullets(Ball newBullet)
    {
       
        if (_counter.CurrentState == Counter.CounterState.STOP)
        {
            _counter.StartTimerUpdatePercentage(0.1f, () =>
                {
                    LoadDoneBullets(_preBullet, newBullet);
                }, null);
        }
    }

    public void LoadDoneBullets(Ball first, Ball second)
    {
        //Debug.Log("First Bullet " + first.GetBallColor() + " second bullet " + second.GetBallColor());
        _bullet = first;
        _preBullet = second;

        _bullet.transform.parent = BulletTransform;
        _bullet.transform.localPosition = Vector3.zero;

        _preBullet.transform.parent = PreBulletTransform;
        _preBullet.transform.localPosition = Vector3.zero;

        _isGunReady = true;
    }

    public void BlockGun()
    {
        _isBlock = true;
    }

    public void UnBlockGun()
    {
        _isBlock = false;
    }
}
