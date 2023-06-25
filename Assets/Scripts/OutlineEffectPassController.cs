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
    public float NormalThresholdMin = 0f;
    public float NormalThresholdMax = 1f;
    public float MinNormalDistance = 0f;
    public float MaxNormalDistance = 1f;
    public float DepthThreshold = 0f;
    public float MinDepthinWorldSpace = 0f;
    public float MaxDepthinWorldSpace = 100f;
    public Color OutlineColor = Color.white;
    public float SobelOffset = 0f;
    void Start()
    {
        /*if (IsActive())
        {*/
        EffectMaterial.SetFloat("_NormalThresholdMin", NormalThresholdMin);
        EffectMaterial.SetFloat("_NormalThresholdMax", NormalThresholdMax);
        EffectMaterial.SetFloat("_MinNormalDist", MinNormalDistance);
        EffectMaterial.SetFloat("_MaxNormalDist", MaxNormalDistance);
        EffectMaterial.SetFloat("_DepthThreshold", DepthThreshold);
        EffectMaterial.SetColor("_OutlineColor", OutlineColor);
        EffectMaterial.SetFloat("_SobelOffset", SobelOffset);
        EffectMaterial.SetFloat("_MinDepth", MinDepthinWorldSpace);
        EffectMaterial.SetFloat("_MaxDepth", MaxDepthinWorldSpace);
        //}
    }

    void Update()
    {
        if (DebugMode)
        {
            /*if (IsActive())
            {*/
            EffectMaterial.SetFloat("_NormalThresholdMin", NormalThresholdMin);
            EffectMaterial.SetFloat("_NormalThresholdMax", NormalThresholdMax);
            EffectMaterial.SetFloat("_MinNormalDist", MinNormalDistance);
            EffectMaterial.SetFloat("_MaxNormalDist", MaxNormalDistance);
            EffectMaterial.SetFloat("_DepthThreshold", DepthThreshold);
            EffectMaterial.SetColor("_OutlineColor", OutlineColor);
            EffectMaterial.SetFloat("_SobelOffset", SobelOffset);
            EffectMaterial.SetFloat("_MinDepth", MinDepthinWorldSpace);
            EffectMaterial.SetFloat("_MaxDepth", MaxDepthinWorldSpace);
            //}
        }
    }

    /*public bool IsActive()
    {
        if (NormalThresholdMin == 0 && OutlineColor == Color.white && SobelOffset == 0)
            return false;
        else
            return true;
    }*/

}
