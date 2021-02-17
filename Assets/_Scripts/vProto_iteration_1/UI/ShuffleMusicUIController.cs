using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Image = UnityEngine.UI.Image;

public class ShuffleMusicUIController : MonoBehaviour
{
    [Header("Visuales")]
    public Image holderImg;

    [Header("Cooldown")]
    public float timeCooldown = 10f;
    public bool btnEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        if (holderImg == null)
            holderImg = GetComponent<Image>();

    }

    public void ButtonShufflePressed()
    {
        if (btnEnabled)
        {
            StartCoroutine(StartCooldown());
            ShuffleMusicManager.instance.onShuffleMusic.Invoke(ShuffleOptions.Reset);
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
