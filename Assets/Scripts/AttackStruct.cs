using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Attack
{
    public string animatorTriggerName;
    //[HideInInspector]
    public int triggerHash;
    public float damage;
    //public float knockback;

    public void Initialize()
    {
        triggerHash = Animator.StringToHash(animatorTriggerName);
    }
}
