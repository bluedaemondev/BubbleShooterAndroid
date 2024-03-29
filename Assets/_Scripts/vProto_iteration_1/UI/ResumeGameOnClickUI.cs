﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class ResumeGameOnClickUI : MonoBehaviour
{
    Button btnComponent;

    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();
        btnComponent.onClick.AddListener(ResumeGame);
    }

    void ResumeGame()
    {
        StartCoroutine(DelayedResume());
    }
    IEnumerator DelayedResume()
    {
        yield return new WaitForSeconds(1f);
        GameManagerActions.instance.onResumeGame.Invoke();

    }
}
