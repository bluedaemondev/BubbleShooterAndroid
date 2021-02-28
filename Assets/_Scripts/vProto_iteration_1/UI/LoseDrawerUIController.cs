using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseDrawerUIController : MonoBehaviour
{
    public void RetryBtn()
    {
        //SceneManager.LoadScene(4); 
        //GameManagerActions.instance.onLoadGameScene.Invoke();

        ProgressSceneLoader.instance.LoadScene(SceneManager.GetActiveScene().name);
        // necesario resetear todos los valores por default y no recargar la escena
        // para no perder las utils.
    }

}
