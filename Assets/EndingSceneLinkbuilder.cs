using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneLinkbuilder : MonoBehaviour
{
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}
