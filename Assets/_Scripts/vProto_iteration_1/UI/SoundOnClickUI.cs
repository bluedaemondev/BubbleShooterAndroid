using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Button = UnityEngine.UI.Button;
[RequireComponent(typeof(Button))]
public class SoundOnClickUI : MonoBehaviour
{
    public AudioClip clipToPlay;
    Button btnComponent;

    // Start is called before the first frame update
    void Start()
    {
        btnComponent = GetComponent<Button>();
        btnComponent.onClick.AddListener(PlayClip);
    }
    void PlayClip()
    {
        SoundManager.instance.PlayEffect(clipToPlay);
    }
}
