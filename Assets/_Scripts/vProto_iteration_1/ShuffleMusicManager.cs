using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShuffleMusicManager : MonoBehaviour
{
    public static ShuffleMusicManager instance { get; private set; }

    public UnityEvent onShuffleMusic;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;

        onShuffleMusic = new UnityEvent();
    }

}
