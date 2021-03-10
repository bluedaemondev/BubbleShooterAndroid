using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundImageManager : MonoBehaviour
{
    public static BackgroundImageManager instance { get; private set; }

    [Header("Animator para transiciones de fondo")]
    public Animator animator;

    [Header("Todos los backgrounds disponibles")]
    public List<Sprite> listAllAvailableBackgrounds;

    [Header("Cantidad de backgrounds cargados en escena")]
    public int availableBackgroundsInRun = 5;
    List<Sprite> sceneBackgrounds;

    [HideInInspector]
    public int currentIndexSelected = 0;
    private SpriteRenderer sprRendBg;

    void Awake()
    {
        if (!instance)
            instance = this;

        if (!animator)
        {
            animator = transform.parent.GetComponent<Animator>();
        }
    }

    public void StartShuffleMechanic()
    {
        sprRendBg = GetComponent<SpriteRenderer>();
        LoadSceneBackgroundList();
        ShuffleMusicManager.instance.onShuffleMusic.AddListener(ChangeBackground);
    }

    private void LoadSceneBackgroundList()
    {
        sceneBackgrounds = new List<Sprite>();
        if (listAllAvailableBackgrounds.Count != 0)
        {
            //Cargo la cantidad requerida evitando loops repetidos
            for (int i = 0; i < availableBackgroundsInRun; i++)
            {
                int rndPick = 0;
                do
                {
                    rndPick = Random.Range(0, listAllAvailableBackgrounds.Count);
                } while (sceneBackgrounds.Contains(listAllAvailableBackgrounds[rndPick]));
                sceneBackgrounds.Add(listAllAvailableBackgrounds[rndPick]);
            }
            sprRendBg.sprite = sceneBackgrounds[currentIndexSelected];
        }
    }

    public void ChangeBackground(ShuffleOptions toDo)
    {
        switch (toDo)
        {
            // pasa a la siguiente cancion
            case ShuffleOptions.Next:
                currentIndexSelected = Mathf.Clamp(currentIndexSelected + 1, 0, sceneBackgrounds.Count - 1);
                this.sprRendBg.sprite = sceneBackgrounds[currentIndexSelected];
                animator.Play("changingBackground");
                break;

            // arma una nueva lista con los loops
            case ShuffleOptions.Reset:
                currentIndexSelected = 0;
                LoadSceneBackgroundList();
                break;

            // elige un indice actual aleatorio dentro de la lista?
            case ShuffleOptions.Random:
                Debug.Log("Random picked shuffle (idx) " + this.name);
                break;


        }

    }
}
