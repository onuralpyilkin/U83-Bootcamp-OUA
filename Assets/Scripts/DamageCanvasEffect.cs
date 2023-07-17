using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanvasEffect : MonoBehaviour
{
    public static DamageCanvasEffect Instance;
    public CanvasGroup canvasGroup;
    public float pulseDuration = 1f;
    private float alphaTarget = 0.5f;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    public void pulse()
    {
        StopAllCoroutines();
        StartCoroutine(PulseCoroutine());
    }

    IEnumerator PulseCoroutine()
    {
        float startTime = Time.time;
        while (Time.time - startTime < pulseDuration)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, pulseDuration * Time.deltaTime);
            if (canvasGroup.alpha == alphaTarget)
            {
                if (alphaTarget == 0f)
                    alphaTarget = 0.5f;
                else
                    alphaTarget = 0f;
            }
            yield return null;
        }
        while (canvasGroup.alpha != 0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, pulseDuration * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
}
