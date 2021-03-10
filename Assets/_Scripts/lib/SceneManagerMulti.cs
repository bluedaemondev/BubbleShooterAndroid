using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerMulti : MonoBehaviour
{
    public string staticFolderName = "Static";
    public int staticSceneCountInBuildSettings = 3;

    // Start is called before the first frame update
    void Start()
    {
        var thisScene = SceneManager.GetActiveScene();

        // load all scenes
        for (int i = 0; i < staticSceneCountInBuildSettings; i++)
        {
            // skip if is current scene since we don't want it twice
            if (thisScene.buildIndex == i) continue;

            // Skip if scene is already loaded
            if (SceneManager.GetSceneByBuildIndex(i).IsValid()) continue;

            //var sc = SceneManager.GetSceneByBuildIndex(i);
            //print(sc.name);

            //if (sc.path.Contains(staticFolderName))
            //{
            SceneManager.LoadScene(i, LoadSceneMode.Additive);

            //}
            // or depending on your usecase
            //SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
        }

    }
}
