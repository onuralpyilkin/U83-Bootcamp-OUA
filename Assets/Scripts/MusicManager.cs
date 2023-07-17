using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    MusicManager Instance;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    public float switchTime = 1f;

    private void Awake()
    {
        Instance = Instance != null ? Instance : this;
        audioSource1 = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource1.volume = 0f;
        audioSource2 = transform.GetChild(1).GetComponent<AudioSource>();
        audioSource2.volume = 0f;
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (!audioSource1.isPlaying)
        {
            audioSource1.clip = audioClip;
            StopAllCoroutines();
            StartCoroutine(FadeInMusicCoroutine(audioSource1, switchTime));
        }
        else
        {
            StopAllCoroutines();
            AudioSource temp = audioSource1;
            audioSource1 = audioSource2;
            audioSource2 = temp;
            audioSource1.clip = audioClip;
            StartCoroutine(FadeOutMusicCoroutine(audioSource2, switchTime));
            StartCoroutine(FadeInMusicCoroutine(audioSource1, switchTime));
        }
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        if(audioSource1.isPlaying)
            StartCoroutine(FadeOutMusicCoroutine(audioSource1, switchTime));
        if (audioSource2.isPlaying)
            StartCoroutine(FadeOutMusicCoroutine(audioSource2, switchTime));
    }

    public void VolumeChangeSmooth(float volume, float lerpTime)
    {
        StartCoroutine(VolumeLerpCoroutine(volume, lerpTime));
    }

    IEnumerator VolumeLerpCoroutine(float volume, float lerpTime)
    {
        float startTime = Time.time;
        while (audioSource1.volume != volume && Time.time - startTime < lerpTime)
        {
            audioSource1.volume = Mathf.Lerp(audioSource1.volume, volume, (Time.time - startTime) / lerpTime);
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeInMusicCoroutine(AudioSource audioSource, float switchTime)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        float startTime = Time.time;
        while (audioSource.volume < 1f && Time.time - startTime < switchTime)
        {
            audioSource.volume = Mathf.Clamp((Time.time - startTime) / switchTime, 0f, 1f);
            yield return null;
        }
        yield break;
    }

    IEnumerator FadeOutMusicCoroutine(AudioSource audioSource, float switchTime)
    {
        float startTime = Time.time;
        while (audioSource.volume > 0f && Time.time - startTime < switchTime)
        {
            audioSource.volume = Mathf.Clamp(1 - (Time.time - startTime) / switchTime, 0f, 1f);
            yield return null;
        }
        audioSource.Stop();
        yield break;
    }
}
