using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class OutlineEffectPassController : MonoBehaviour
{
    //private string featureName = "OutlinePassRendererFeature";
    public Material effectMaterial;
    public bool debugMode = false;
    public int dotsDensity = 0;

    [Range(0f, 1f)]
    public float dotsCutOff = 0f;
    public Vector2 scrollDirection = new Vector2();
    public float normalThreshold = 0f;
    public float depthThreshold = 0f;
    public Color outlineColor = Color.white;
    public float sobelOffset = 0f;
    void Start()
    {
        if (IsActive())
        {
            effectMaterial.SetInt("_IsActivated", 1);
            effectMaterial.SetInt("_DotsDensity", dotsDensity);
            effectMaterial.SetFloat("_DotsCutOff", dotsCutOff);
            effectMaterial.SetVector("_ScrollDirection", scrollDirection);
            effectMaterial.SetFloat("_NormalThreshold", normalThreshold);
            effectMaterial.SetFloat("_DepthThreshold", depthThreshold);
            effectMaterial.SetColor("_OutlineColor", outlineColor);
            effectMaterial.SetFloat("_SobelOffset", sobelOffset);
        }
        else
        {
            effectMaterial.SetInt("_IsActivated", 0);
        }
    }

    void Update()
    {
        if (debugMode)
        {
            if (IsActive())
            {
                effectMaterial.SetInt("_IsActivated", 1);
                effectMaterial.SetInt("_DotsDensity", dotsDensity);
                effectMaterial.SetFloat("_DotsCutOff", dotsCutOff);
                effectMaterial.SetVector("_ScrollDirection", scrollDirection);
                effectMaterial.SetFloat("_NormalThreshold", normalThreshold);
                effectMaterial.SetFloat("_DepthThreshold", depthThreshold);
                effectMaterial.SetColor("_OutlineColor", outlineColor);
                effectMaterial.SetFloat("_SobelOffset", sobelOffset);
            }
            else
            {
                effectMaterial.SetInt("_IsActivated", 0);
            }
        }
    }

    public bool IsActive()
    {
        if (dotsDensity == 0 && dotsCutOff == 0 && scrollDirection == Vector2.zero && normalThreshold == 0 && outlineColor == Color.white && sobelOffset == 0)
            return false;
        else
            return true;
    }

}
