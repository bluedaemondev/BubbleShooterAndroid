using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCanvasUIController : MonoBehaviour
{
    public CanvasGroup cgPopup;
    public Canvas canvasOrigin;
    public bool shown = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!cgPopup)
            this.cgPopup = GetComponentInChildren<CanvasGroup>();
        
        this.cgPopup.alpha = 0;
        SubscribeToCorrespondingEvent();
    }

    public virtual void SubscribeToCorrespondingEvent()
    {
        Debug.Log("Setting popup");
    }

    public void ActivatePopup()
    {

        if (!shown)
        {
            GameManagerActions.instance.onPause.Invoke();
            StartCoroutine(TransitionPopupShow());
            shown = true;
            this.canvasOrigin.sortingOrder = 10;
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
