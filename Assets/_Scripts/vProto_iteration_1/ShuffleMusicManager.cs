using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShuffleMusicManager : MonoBehaviour
{
    public static ShuffleMusicManager instance { get; private set; }

    public UnityEvent<ShuffleOptions> onShuffleMusic;
    public int availableLoopsInRun = 5;
    [Header("Tiempo de repeticion por loop")]
    public float timeRepeatLoop = 10f;

    [Header("Todos los loops de audio disponibles")]
    public List<AudioClip> listAllAvailableLoops;
    List<AudioClip> sceneLoops;

    [HideInInspector]
    public int currentIndexSelected = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onShuffleMusic = new UnityEvent<ShuffleOptions>();
    }

    void Start()
    {
        LoadSceneLoopList();
        ShuffleMusicManager.instance.onShuffleMusic.AddListener(ChangeLoops);
        InvokeRepeating("ChangeMusicLoop", timeRepeatLoop, timeRepeatLoop);
    }
    private void ChangeMusicLoop()
    {
        onShuffleMusic.Invoke(ShuffleOptions.Next);
    }

    private void LoadSceneLoopList()
    {
        sceneLoops = new List<AudioClip>();
        if (listAllAvailableLoops.Count != 0)
        {
            //Cargo la cantidad requerida evitando loops repetidos
            for (int i = 0; i < availableLoopsInRun; i++)
            {
                int rndPick = 0;
                do
                {
                    rndPick = Random.Range(0, listAllAvailableLoops.Count);
                } while (sceneLoops.Contains(listAllAvailableLoops[rndPick]));
                sceneLoops.Add(listAllAvailableLoops[rndPick]);
            }
        }
    }

    public void ChangeLoops(ShuffleOptions toDo)
    {
        switch (toDo)
        {
            // pasa a la siguiente cancion
            case ShuffleOptions.Next:
                currentIndexSelected = Mathf.Clamp(currentIndexSelected + 1, 0, sceneLoops.Count - 1);
                if(currentIndexSelected == sceneLoops.Count - 1)
                {
                    Debug.Log("Terminados todos los loops, saliendo.");
                }

                break;

            // arma una nueva lista con los loops
            case ShuffleOptions.Reset:
                currentIndexSelected = 0;
                LoadSceneLoopList();
                break;

            // elige un indice actual aleatorio dentro de la lista?
            case ShuffleOptions.Random:
                Debug.Log("Random picked shuffle (idx) " + this.name);
                break;


        }

    }

}
