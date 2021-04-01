using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class ShuffleMusicOnClickUI : MonoBehaviour
{
    Button btnComponent;
    
    void Start()
    {
        btnComponent = GetComponent<Button>();

        btnComponent.onClick.AddListener(ShuffleEvent);

    }

    void ShuffleEvent()
    {
        ShuffleMusicManager.instance.onShuffleMusic.Invoke(ShuffleOptions.Reset);
    }
}
