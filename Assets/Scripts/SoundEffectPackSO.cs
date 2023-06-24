using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Effect Pack", menuName = "Sound Effect Pack")]
public class SoundEffectPack : ScriptableObject
{
    public string SoundEffectPackName;
    public List<AudioClip> SoundEffects = new List<AudioClip>();

    public AudioClip GetRandomSoundEffect()
    {
        return SoundEffects[Random.Range(0, SoundEffects.Count)];
    }
}
