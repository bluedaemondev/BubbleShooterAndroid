using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Text = TMPro.TextMeshProUGUI;

public class PointsMultiplierUIController : MonoBehaviour
{
    [Header("String mostrado antes de cantidad de puntos"), TextArea(0, 3)]
    public string precedingString = "x";
    
    private Text textComponent;
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        PointsManager.instance.onMultiplierUpdate.AddListener(SetUiText);

    }
    public void SetUiText(float multiplier)
    {
        this.textComponent.text = precedingString + multiplier.ToString("0.0");
    }
}
