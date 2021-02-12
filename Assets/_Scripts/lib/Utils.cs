using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils instance { get; private set; }

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    public Vector3 MouseToWorldWithoutZ()
    {
        Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        result.z = 0;
        return result;
    }
}
