using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class OutlineEffectPassController : MonoBehaviour
{
    //private string featureName = "OutlinePassRendererFeature";
    public Material EffectMaterial;
    public bool DebugMode = false;
    public float NormalThreshold = 0f;
    public float DepthThreshold = 0f;
    public float MinDepthinWorldSpace = 0f;
    public float MaxDepthinWorldSpace = 100f;
    public Color OutlineColor = Color.white;
    public float SobelOffset = 0f;
    void Start()
    {
        if (IsActive())
        {
            EffectMaterial.SetInt("_IsActivated", 1);
            EffectMaterial.SetFloat("_NormalThreshold", NormalThreshold);
            EffectMaterial.SetFloat("_DepthThreshold", DepthThreshold);
            EffectMaterial.SetColor("_OutlineColor", OutlineColor);
            EffectMaterial.SetFloat("_SobelOffset", SobelOffset);
            EffectMaterial.SetFloat("_MinDepth", MinDepthinWorldSpace);
            EffectMaterial.SetFloat("_MaxDepth", MaxDepthinWorldSpace);
        }
        else
        {
            EffectMaterial.SetInt("_IsActivated", 0);
        }
    }

    void Update()
    {
        if (DebugMode)
        {
            if (IsActive())
            {
                EffectMaterial.SetInt("_IsActivated", 1);
                EffectMaterial.SetFloat("_NormalThreshold", NormalThreshold);
                EffectMaterial.SetFloat("_DepthThreshold", DepthThreshold);
                EffectMaterial.SetColor("_OutlineColor", OutlineColor);
                EffectMaterial.SetFloat("_SobelOffset", SobelOffset);
                EffectMaterial.SetFloat("_MinDepth", MinDepthinWorldSpace);
                EffectMaterial.SetFloat("_MaxDepth", MaxDepthinWorldSpace);
            }
            else
            {
                EffectMaterial.SetInt("_IsActivated", 0);
            }
        }
    }

    public bool IsActive()
    {
        if (NormalThreshold == 0 && OutlineColor == Color.white && SobelOffset == 0)
            return false;
        else
            return true;
    }

}
