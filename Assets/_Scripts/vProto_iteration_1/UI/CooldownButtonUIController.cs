using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(Button))]

public class CooldownButtonUIController : MonoBehaviour
{
    [Header("Visuales")]
    public Image holderImg;
    private Button btnComponent;


    [Header("Cooldown")]
    public float timeCooldown = 10f;
    public bool btnEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        if (holderImg == null)
            holderImg = GetComponent<Image>();
        btnComponent = GetComponent<Button>();

        btnComponent.onClick.AddListener(CooldownOnCLick);

    }

    void CooldownOnCLick()
    {
        if (btnEnabled)
        {
            StartCoroutine(StartCooldown());
        }

    }

    private IEnumerator StartCooldown()
    {
        btnEnabled = false;
        Debug.Log("Starting coroutine cooldown " + this.gameObject.name);

        float currTimer = 0f;
        while (currTimer <= timeCooldown)
        {
            currTimer += Time.unscaledDeltaTime;
            holderImg.fillAmount = Mathf.Clamp01(currTimer / timeCooldown);
            yield return null;
        }

        Debug.Log("Ending coroutine cooldown " + this.gameObject.name);
        btnEnabled = true;
    }

}
