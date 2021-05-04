using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopAsset_", menuName = "Loop asset", order = 0)]
public class BackgroundAndMusic : ScriptableObject
{
    public AudioClip ClipAssociated; 
    public string AnimatorStateName;
    public float timeRepeats;

}
