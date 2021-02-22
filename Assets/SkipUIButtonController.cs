using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;
public class SkipUIButtonController : MonoBehaviour
{
    [Header("Tiempo hasta poner disponible el boton")]
    public float timeEnableBtn = 4f;

    private Button buttonComponent;


    private void Start()
    {
        this.buttonComponent = GetComponent<Button>();
        Debug.Log(buttonComponent);
        StartCoroutine(WaitForEnable());
    }

    private IEnumerator WaitForEnable()
    {
        buttonComponent.image.CrossFadeAlpha(1, timeEnableBtn, false);
        yield return new WaitForSeconds(timeEnableBtn);
        this.buttonComponent.interactable = true;
    }
}
