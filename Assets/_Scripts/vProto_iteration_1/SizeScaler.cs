using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float width = ScreenSize.GetScreenToWorldWidth;
        transform.localScale = Vector3.one * width;

        Vector3 normalizedPos = Camera.main.transform.position;
        normalizedPos.z = 0;
        transform.position = normalizedPos;
        //transform.position = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
