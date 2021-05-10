using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterSceneActions : MonoBehaviour
{
    //public ShooterSceneResources sceneResources;

    void Start()
    {
        SetUpRequiredMechanics();
        if (FindObjectOfType<AdmobCanvasController>() != null)
        {
            Destroy(FindObjectOfType<AdmobCanvasController>().gameObject);
            Destroy(FindObjectOfType<AdmobBannerScript>().gameObject);
        }
    }

    void SetUpRequiredMechanics()
    {
        ShuffleMusicManager.instance.StartShuffleMechanic();
        //BackgroundImageManager.instance.StartShuffleMechanic();
        //PointsManager.instance.StartAccountance();
    }
}
