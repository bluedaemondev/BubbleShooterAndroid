using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Text = TMPro.TextMeshProUGUI;
using Slider = UnityEngine.UI.Slider;


public class ProgressSceneLoader : MonoBehaviour
{
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private Slider progressBar;

    private AsyncOperation operation;
    private Canvas canvas;

    // Start is called before the first frame update
    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>(true);
        var loaders = FindObjectsOfType<ProgressSceneLoader>();

        if (loaders.Length < 2)
            DontDestroyOnLoad(gameObject);
        else
            for (int extra = 1; extra < loaders.Length; extra++)
                Destroy(loaders[extra].gameObject);
    }

    // Update is called once per frame
    public void LoadScene(string sceneName)
    {
        UpdateProgressUI(0);
        canvas.gameObject.SetActive(true);

        StartCoroutine(BeginLoad(sceneName));

    }

    private IEnumerator BeginLoad(string sceneName)
    {
        var cameraToDisable = FindObjectOfType<Camera>().gameObject;

        operation = GameManagerActions.instance.LoadSceneByNameAsync(sceneName);
        while (!operation.isDone)
        {
            UpdateProgressUI(operation.progress);
            yield return null;
        }
        UpdateProgressUI(operation.progress);
        operation = null;
        canvas.gameObject.SetActive(false);
        //cameraToDisable.SetActive(false);


        var tanda = FindObjectOfType<AdmobInterstitialScript>();

        tanda.RequestAd();
        if (tanda.interstitial.IsLoaded())
        {
            tanda.interstitial.Show();
        }

    }
    private void UpdateProgressUI(float progress)
    {
        progressBar.value = progress;
        progressText.text = "Cargando... \n" + (int)(progress * 100f) + "%";
    }
}
