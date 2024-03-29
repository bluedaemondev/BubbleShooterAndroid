﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShuffleMusicManager : MonoBehaviour
{
    public static ShuffleMusicManager instance { get; private set; }


    public UnityEvent<ShuffleOptions> onShuffleMusic;

    [Header("Cuantos loops se van a cargar como disponibles")]
    public int availableLoopsInRun = 5;
    [Header("Tiempo de repeticion por loop base")]
    public float timeRepeatLoop = 10f;
    // para la mecanica de agregar tiempo despues de pasado un loop
    private float timeRepeatLoopInvoker = 10f;
    [Header("Tiempo a agregar despues de un loop")]
    public float timeAddAfterLoopEnd = 15f;
    [Header("Todos los loops de audio disponibles")]
    public List<BackgroundAndMusic> collectionLoops;


    public AnimatedBackgroundController backgroundController;
    List<BackgroundAndMusic> sceneLoops;
    int currentIndexSelected = 0;

    public GameManager gameManager;


    public float TotalLoopTime
    {
        get { return sceneLoops[currentIndexSelected].ClipAssociated.length; }
    }
    public int CurrentIndex
    {
        get { return currentIndexSelected; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onShuffleMusic = new UnityEvent<ShuffleOptions>();

    }
    /// <summary>
    /// Inicio de mecanica de shuffle / recalculo de lista de musica disponible
    /// </summary>
    public void StartShuffleMechanic()
    {
        LoadSceneLoopList();

        //timeRepeatLoopInvoker = CalculateInvokerTime();
        onShuffleMusic.AddListener(ChangeLoops);
        StartCoroutine(ChangeBasedOnInvokeTime());
    }
    public void ChangeLoops(ShuffleOptions toDo)
    {
        switch (toDo)
        {
            // pasa a la siguiente cancion
            case ShuffleOptions.Next:
                currentIndexSelected++;
                if (currentIndexSelected >= sceneLoops.Count) // termino y se sale
                    gameManager.OnWin();

                else
                    PlayBackgroundAndMusic(sceneLoops[currentIndexSelected]);
                break;

            // arma una nueva lista con los loops
            case ShuffleOptions.Reset:
                currentIndexSelected = 0;
                LoadSceneLoopList();
                break;

            // elige un indice actual aleatorio dentro de la lista?
            case ShuffleOptions.Random:
                //Debug.Log("Random picked shuffle (idx) " + this.name);
                break;
        }

    }
    private IEnumerator ChangeBasedOnInvokeTime()
    {
        while (currentIndexSelected < sceneLoops.Count)
        {
            var tmp = sceneLoops[currentIndexSelected].timeRepeats;
            Debug.Log("tmp = " + tmp + " , timer prev : " + timeRepeatLoopInvoker);


            yield return new WaitForSeconds(tmp);

            ChangeMusicLoop();
        }

    }

    private void ChangeMusicLoop()
    {
        timeRepeatLoopInvoker = CalculateInvokerTime();
        onShuffleMusic.Invoke(ShuffleOptions.Next);
    }
    private float CalculateInvokerTime()
    {
        return timeRepeatLoop + timeAddAfterLoopEnd * currentIndexSelected;
    }

    private void LoadSceneLoopList()
    {
        sceneLoops = new List<BackgroundAndMusic>();
        if (collectionLoops.Count > 0)
        {
            //Cargo la cantidad requerida evitando loops repetidos
            for (int i = 0; i < (availableLoopsInRun <= collectionLoops.Count ? availableLoopsInRun : collectionLoops.Count); i++)
            {
                sceneLoops.Add(collectionLoops[i]);
            }
            PlayBackgroundAndMusic(sceneLoops[currentIndexSelected]);
        }
    }
    private void PlayBackgroundAndMusic(BackgroundAndMusic clip)
    {
        SoundManager.instance.PlayMusic(clip.ClipAssociated);
        backgroundController.PlayState(clip.AnimatorStateName);
    }



}
