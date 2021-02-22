using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RectTransform))]

public class PauseDrawerUIController : MonoBehaviour
{
    RectTransform rectTransform;

    #region Getter
    static PauseDrawerUIController instance;
    public static PauseDrawerUIController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PauseDrawerUIController>();
            if (instance == null)
                Debug.LogError("PauseDrawerUIController not found");
            return instance;
        }
    }
    #endregion Getter

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(rectTransform.rect.width, 0f);
        Hide();
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f, false);
        GameManagerActions.instance.onPause.Invoke();
    }

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(-rectTransform.rect.width * 3, 0.3f).SetDelay(delay);
        GameManagerActions.instance.onResumeGame.Invoke();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // hardcode 0
    }
}
