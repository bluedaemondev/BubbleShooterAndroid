using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

/// <summary>
/// Probablemente deprecado
/// </summary>
public class HudController : MonoBehaviour
{
    public static HudController instance { get; private set; }

    [Header("Control de puntos")]
    public Text pointsText;
    public Slider pointsSlider;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        
    }
    
    

}
