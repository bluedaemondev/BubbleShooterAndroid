using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterSceneActions : MonoBehaviour
{
    public Camera mainCamScene;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerActions.instance.onPause.Invoke();

        SetUpRequiredMechanics();
        Utils.instance.SetMainCamera(mainCamScene);
    }

    void SetUpRequiredMechanics()
    {
        ShuffleMusicManager.instance.StartShuffleMechanic();
        BackgroundImageManager.instance.StartShuffleMechanic();
        PointsManager.instance.StartAccountance();
    }
}
