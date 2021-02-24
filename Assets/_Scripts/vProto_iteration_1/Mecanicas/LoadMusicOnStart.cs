using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMusicOnStart : MonoBehaviour
{
    public AudioClip musicOnStart;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayMusic(musicOnStart);
    }
}
