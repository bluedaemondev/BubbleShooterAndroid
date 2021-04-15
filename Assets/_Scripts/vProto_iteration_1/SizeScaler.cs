using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        float width = FindObjectOfType<ScreenSize>().GetScreenToWorldWidth;
        transform.localScale = Vector3.one * width;

        Vector3 normalizedPos = Utils.instance.MainCam.transform.position;
        normalizedPos.z = 0;
        transform.position = normalizedPos;
        //transform.position = 
    }
}
