/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{

    private RawImage _texture;
    private float _slide;
    private bool _isPlaying;
    private Counter _counter;
    // Use this for initialization
    void Awake()
    {
        _isPlaying = true;
        _counter = GetComponent<Counter>();
        _texture = GetComponent<RawImage>();
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlaying)
        {
            updateTileXTexture();
        }
    }

    void updateTileXTexture()
    {
        _slide += Time.fixedDeltaTime * 0.01f;
        _texture.uvRect = new Rect(_slide, 0, 1, 1);
    }

    public void RedAlert()
    {
        //Debug.Log("Alert");
        if (_counter.CurrentState == Counter.CounterState.STOP)
        {
            _counter.StartTimerUpdatePercentage(0.8f, () =>
                {
                    _texture.color = Color.white;
                }, ((float percent) =>
                    {
                        _texture.color = Color.Lerp(Color.white, new Color(1,0.5f,0.5f,1), percent);
                    }));
        }
    }

    public void NormalMode()
    {
        _texture.color = Color.white;
        _isPlaying = true;
    }

    public void StopMode(){
        _texture.color = Color.gray;
        _counter.StopTimer();
        _isPlaying = false;
    }
}
