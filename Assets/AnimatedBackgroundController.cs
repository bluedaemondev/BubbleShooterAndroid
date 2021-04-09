using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBackgroundController : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayState(string stateName)
    {
        if (animator)
        {
            animator.Play(stateName);
            Debug.Log("Playing state " + stateName);
        }

    }

}
