using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCanvasUIController : MonoBehaviour
{
    public CanvasGroup cgPopup;
    bool shown = false;
    // Start is called before the first frame update
    void Start()
    {
        this.cgPopup = GetComponentInChildren<CanvasGroup>();
        this.cgPopup.alpha = 0;
        GameManagerActions.instance.defeatEvent.AddListener(ActivatePopup);
    }

    public void ActivatePopup()
    {

        if (!shown)
        {
            GameManagerActions.instance.onPause.Invoke();
            StartCoroutine(TransitionPopupShow());
            shown = true;
        }
    }

    IEnumerator TransitionPopupShow()
    {
        while (cgPopup.alpha < 1)
        {
            cgPopup.alpha += 0.1f;
            yield return null;
        }
    }



}
