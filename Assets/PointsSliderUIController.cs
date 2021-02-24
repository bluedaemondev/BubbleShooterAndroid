using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slider = UnityEngine.UI.Slider;

[RequireComponent(typeof(Slider))]
public class PointsSliderUIController : MonoBehaviour
{
    Slider compSlider;
    float currValue;

    // Start is called before the first frame update
    void Start()
    {
        compSlider = GetComponent<Slider>();
        PointsManager.instance.onPointsUpdate.AddListener(SetUiValue);
    }
    /// <summary>
    /// Se calcula en base a 3 puntos clave (estrellas de puntaje)
    /// Terminar implementacion con relevamiento sobre mecanica posible
    /// </summary>
    /// <param name="points"></param>
    public void SetUiValue(int points)
    {
        this.compSlider.value = points;
    }
}
