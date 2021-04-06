using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopAsset_", menuName = "Loop asset", order = 0)]
public class BackgroundAndMusic : ScriptableObject
{
    private Sprite spriteBackground;
    private AudioClip clipAssociated;
    private string animatorStateName;

    public Sprite SpriteBackground; /*{ get => spriteBackground; set => spriteBackground = value; }*/
    public AudioClip ClipAssociated;/* { get => clipAssociated; set => clipAssociated = value; }*/
    public string AnimatorStateName; /* { get => animatorStateName; set => animatorStateName = value; }*/


}
