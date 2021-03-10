using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour
{
    [Header("Camara desde donde escalo el objeto.")]
    public Transform scaleFrom;
    // Start is called before the first frame update
    void Start()
    {
        float width = ScreenSize.GetScreenToWorldWidth;
        transform.localScale = Vector3.one * width;

        Vector3 normalizedPos = scaleFrom.position;
        normalizedPos.z = 0;
        transform.position = normalizedPos;
        //transform.position = 
    }
}
