using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

[RequireComponent(typeof(Text))]
public class PointsTextUIController : MonoBehaviour
{
    [Header("String mostrado antes de cantidad de puntos"), TextArea(0, 3)]
    public string precedingString = "";
    [Header("String mostrado despues de cantidad de puntos"), TextArea(0, 3)]
    public string posteriorString = "";

    Text textComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.textComponent = GetComponent<Text>();
        PointsManager.instance.onPointsUpdate.AddListener(SetUiText);
    }

    public void SetUiText(int points)
    {
        this.textComponent.text = precedingString + points.ToString() + posteriorString;
    }

}
