using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BubbleType
{
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


    public static BubbleType GenerateRandomBubbleType(bool useSpecials = false)
    {
        var sourceList = BubbleResources.instance.bubbleResources;
        if (useSpecials)
        {
            sourceList.AddRange(BubbleResources.instance.specialBubbleResources);
        }
        int randSelection = Random.Range(0, sourceList.Count);

        return sourceList[randSelection];
    }
    public static BubbleType GenerateSpecialBubbleType()
    {
        var sourceList = BubbleResources.instance.specialBubbleResources;
        int randSelection = Random.Range(0, sourceList.Count);
        return sourceList[randSelection];
    }
}
