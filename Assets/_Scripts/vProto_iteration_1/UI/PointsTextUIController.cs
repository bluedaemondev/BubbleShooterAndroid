using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
public class PointsTextUIController : MonoBehaviour
{
    public Text textComponent;
    // Start is called before the first frame update
    void Start()
    {
        this.textComponent = GetComponent<Text>();
        SetUiText();
    }

    public void SetUiText()
    {
        this.textComponent.text = "Tu puntuacion: " + PointsManager.instance.GetTotalPoints();
    }
    
}
