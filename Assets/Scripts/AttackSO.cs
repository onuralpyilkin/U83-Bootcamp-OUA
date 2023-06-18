using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string AttackName;
    public string AnimatorTriggerName;
    public float Damage;
    public float Range;
    //public float knockback;

    [HideInInspector]
    public int TriggerHash = 0;

    public void Initialize()
    {
        TriggerHash = Animator.StringToHash(AnimatorTriggerName);
    }
}
