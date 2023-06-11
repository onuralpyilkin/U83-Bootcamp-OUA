using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string attackName;
    public string animatorTriggerName;
    public float damage;
    //public float knockback;

    [HideInInspector]
    public int triggerHash = 0;

    public void Initialize()
    {
        triggerHash = Animator.StringToHash(animatorTriggerName);
    }
}
