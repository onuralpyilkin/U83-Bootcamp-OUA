void sobelNormal_float(float2 upLeft, float2 up, float2 upRight, float2 left, float2 right, float2 downLeft, float2 down, float2 downRight, out float sobel)
{
        float3 upLeftC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(upLeft);
        float3 upC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(up);
        float3 upRightC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(upRight);
        float3 leftC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(left);
        float3 rightC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(right);
        float3 downLeftC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(downLeft);
        float3 downC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(down);
        float3 downRightC = SHADERGRAPH_SAMPLE_SCENE_NORMAL(downRight);

        float sobelXr = upLeftC.x * -1 + upC.x * -2 + upRightC.x * -1
                + downLeftC.x * 1 + downC.x * 2 + downRightC.x * 1;
        
        float sobelYr = upLeftC.x * -1 + upRightC.x * 1
                + leftC.x * -2 + rightC.x * 2
                + downLeftC.x * -1 + downRightC.x * 1;

        float sobelXg = upLeftC.y * -1 + upC.y * -2 + upRightC.y * -1
                + downLeftC.y * 1 + downC.y * 2 + downRightC.y * 1;

        float sobelYg = upLeftC.y * -1 + upRightC.y * 1
                + leftC.y * -2 + rightC.y * 2
                + downLeftC.y * -1 + downRightC.y * 1;
        
        float sobelXb = upLeftC.z * -1 + upC.z * -2 + upRightC.z * -1
                + downLeftC.z * 1 + downC.z * 2 + downRightC.z * 1;
        
        float sobelYb = upLeftC.z * -1 + upRightC.z * 1
                + leftC.z * -2 + rightC.z * 2
                + downLeftC.z * -1 + downRightC.z * 1;

        float sobelR = sqrt((sobelXr * sobelXr) + (sobelYr * sobelYr));
        float sobelG = sqrt((sobelXg * sobelXg) + (sobelYg * sobelYg));
        float sobelB = sqrt((sobelXb * sobelXb) + (sobelYb * sobelYb));

        sobel = (sobelR + sobelG + sobelB) / 3;
}

void sobelDepth_float(float2 upLeft, float2 up, float2 upRight, float2 left, float2 right, float2 downLeft, float2 down, float2 downRight, out float sobel)
{
        float upLeftC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(upLeft);
        float upC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(up);
        float upRightC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(upRight);
        float leftC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(left);
        float rightC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(right);
        float downLeftC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(downLeft);
        float downC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(down);
        float downRightC = SHADERGRAPH_SAMPLE_SCENE_DEPTH(downRight);

        float sobelX = upLeftC * -1 + upC * -2 + upRightC * -1
                + downLeftC * 1 + downC * 2 + downRightC * 1;

        float sobelY = upLeftC * -1 + upRightC * 1
                + leftC * -2 + rightC * 2
                + downLeftC * -1 + downRightC * 1;

        sobel = sqrt((sobelX * sobelX) + (sobelY * sobelY));

}

void getSobelUVs_float(float2 center, float offset, out float2 upLeft, out float2 up, out float2 upRight, out float2 left, out float2 right, out float2 downLeft, out float2 down, out float2 downRight)
{
    float xIncrement =  1.0f / 1280.0f * offset;
    float yIncrement =  1.0f / 720.0f * offset;
    upLeft = center + float2(-xIncrement, yIncrement);
    up = center + float2(0, yIncrement);
    upRight = center + float2(xIncrement, yIncrement);
    left = center + float2(-xIncrement, 0);
    right = center + float2(xIncrement, 0);
    downLeft = center + float2(-xIncrement, -yIncrement);
    down = center + float2(0, -yIncrement);
    downRight = center + float2(xIncrement, -yIncrement);   
}

void getSobelOffset_float(float2 uv, float minDepth, float maxDepth, out float offset, out float oneMinusOffset){
        float depthEye = 0;
        if (unity_OrthoParams.w == 1.0)
        {
                depthEye = LinearEyeDepth(ComputeWorldSpacePosition(uv, SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
        }
        else
        {
                depthEye = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv), _ZBufferParams);
        }
        if(depthEye < minDepth)
                depthEye = minDepth;
        if(depthEye > maxDepth)
                depthEye = maxDepth;
        offset = (depthEye - minDepth) / (maxDepth - minDepth);
        oneMinusOffset = 1.0f - offset;
}

void combine_float(float normalSobel, float normalThresholdMin, float normalThresholdMax, float minNormalDist, float maxNormalDist, float depthSobel, float depthThreshold, float4 outlineColor, UnityTexture2D blit, SamplerState ss, float2 uv, float minDepth, float maxDepth, out float4 color){
        float depthEye = 0;
        bool isInDepthRange = false;
        if (unity_OrthoParams.w == 1.0)
        {
                depthEye = LinearEyeDepth(ComputeWorldSpacePosition(uv, SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv), UNITY_MATRIX_I_VP), UNITY_MATRIX_V);
        }
        else
        {
                depthEye = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv), _ZBufferParams);
        }
        if(depthEye <= maxDepth && depthEye >= minDepth)
                isInDepthRange = true;
        if(depthEye < minNormalDist)
                depthEye = minNormalDist;
        if(depthEye > maxNormalDist)
                depthEye = maxNormalDist;
        float oneMinusOffset = 1.0f - ((depthEye - minNormalDist) / (maxNormalDist - minNormalDist));
        bool normalPass = normalSobel > (normalThresholdMin + (oneMinusOffset * (normalThresholdMax - normalThresholdMin)));
        bool depthPass = depthSobel > depthThreshold;
        if((!normalPass && !depthPass) || !isInDepthRange){
                color = SAMPLE_TEXTURE2D(blit, ss, uv);
        }else{
                color = outlineColor;
        }
}