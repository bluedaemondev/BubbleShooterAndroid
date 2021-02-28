using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BubbleType {
    public string type;
    public Sprite sprite;
}

public class BubbleResources : MonoBehaviour
{
    public static BubbleResources instance { get; private set; }

    public List<BubbleType> bubbleResources;
    public List<BubbleType> specialBubbleResources;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
    
}
