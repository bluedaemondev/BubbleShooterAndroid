using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Utils.instance.SetMainCamera(this.GetComponent<Camera>());
    }
}
