using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class ImageSequenceUIController : MonoBehaviour
{
    public static ImageSequenceUIController instance { get; private set; }
    [Header("Paneles o imagenes en secuencia")]
    public List<Sprite> comicSlides;

    [Header("UI Image para mostrar")]
    public Image containerCurrentImg;

    [HideInInspector]
    public int currIdx;
    [Header("Configuracion de eventos disparados en skip de cinematica")]
    public UnityEvent onSkipSequence;

    [SerializeField]string sceneLoadAfter = "Demo";

    private void Awake()
    {
        if (!instance)
            instance = this;

        onSkipSequence = new UnityEvent();
    }
    // Start is called before the first frame update
    void Start()
    {
        onSkipSequence.AddListener(Skip);

        if (!containerCurrentImg)
        {
            Debug.LogError("Image para mostrar no configurada. Agregue un objeto hijo tipo Image a " + this.gameObject.name);
        }
        if (comicSlides.Count > 0)
        {
            this.containerCurrentImg.sprite = comicSlides[0];
        }
    }

    public void GoToNextImage()
    {
        if (currIdx < comicSlides.Count - 2)
        { // quedan imagenes disponibles
            this.currIdx++;
            SetImage(currIdx);
        }
        else
        {
            onSkipSequence.Invoke();
        }

    }

    public void SetImage(int idx)
    {
        this.containerCurrentImg.sprite = comicSlides[currIdx];
    }

    public void Skip()
    {
        this.SetImage(comicSlides.Count - 1);
        //FindObjectOfType<ProgressSceneLoader>().LoadScene(sceneLoadAfter);
        ProgressSceneLoader.instance.LoadScene(sceneLoadAfter);
        SceneManager.UnloadSceneAsync("ComicIntroSlides");

    }
}
