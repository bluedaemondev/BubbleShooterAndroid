using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class HudController : MonoBehaviour
{
    public static HudController current { get; private set; }

    [Header("Control de puntos")]
    public Text pointsText;
    public Slider pointsSlider;


    private void Awake()
    {
        if (current == null)
            current = this;
        
    }
    //private void Start()
    //{
    //}

    public void UpdatePointsUI(int newVal)
    {
        this.pointsText.text = newVal + " points";
        this.pointsSlider.value = newVal;
    }

}
