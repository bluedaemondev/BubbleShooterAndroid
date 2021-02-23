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
        StartCoroutine(WaitForEnable());
    }

    private IEnumerator WaitForEnable()
    {
        if (buttonComponent != null)
        {
            buttonComponent.image.CrossFadeAlpha(1, timeEnableBtn, false);
            yield return new WaitForSeconds(timeEnableBtn);
            this.buttonComponent.interactable = true;
        }
    }

}
