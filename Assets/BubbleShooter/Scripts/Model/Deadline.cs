/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class Deadline : MonoBehaviour {

    GameManager _gameManager;
    Rigidbody2D _rigidBody;

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitLine(GameManager gameManager){
        _gameManager = gameManager;
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals(Common.LAYER_BALL))
        {
            _gameManager.OnGameover();
            _rigidBody.isKinematic = true;
        }
    }

    public void Reset()
    {
        _rigidBody.isKinematic = false;
    }
}
