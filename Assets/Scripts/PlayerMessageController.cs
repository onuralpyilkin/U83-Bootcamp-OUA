using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMessageController : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    public float ClearTime = 5f;
    public float FadeTime = 1f;
    private float alphaTarget = 1f;
    private float clearTimer = 0f;
    private bool isActive = false;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.alpha = 0f;
    }

    void Update()
    {
        if(!isActive)
            return;
        
        clearTimer += Time.deltaTime;
        if(clearTimer >= ClearTime)
        {
            textMesh.alpha = Mathf.MoveTowards(textMesh.alpha, 0f, FadeTime * Time.deltaTime);
            if(textMesh.alpha == 0f)
            {
                isActive = false;
            }
            return;
        }
        textMesh.alpha = Mathf.MoveTowards(textMesh.alpha, alphaTarget, FadeTime * Time.deltaTime);
        if (textMesh.alpha == alphaTarget)
        {
            if(alphaTarget == 0f)
                alphaTarget = 1f;
            else
                alphaTarget = 0f;
        }
    }

    public void SetMessage(string message)
    {
        textMesh.text = message;
        textMesh.alpha = 0f;
        clearTimer = 0f;
        isActive = true;
    }
}
