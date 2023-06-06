void RGBtoHSV_float(float3 In, out float3 Out)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
    float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
    float D = Q.x - min(Q.w, Q.y);
    float  E = 1e-10;
    float V = (D == 0) ? Q.x : (Q.x + E);
    Out = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
}

void sobelNormal_float(float3 center, float3 upLeft, float3 up, float3 upRight, float3 left, float3 right, float3 downLeft, float3 down, float3 downRight, out float sobel){
    float sobelXr = upLeft.x * -1 + up.x * -2 + upRight.x * -1
            + left.x * 0 + center.x * 0 + right.x * 0
            + downLeft.x * 1 + down.x * 2 + downRight.x * 1;

    float sobelYr = upLeft.x * -1 + up.x * 0 + upRight.x * 1
            + left.x * -2 + center.x * 0 + right.x * 2
            + downLeft.x * -1 + down.x * 0 + downRight.x * 1;

    float sobelXg = upLeft.y * -1 + up.y * -2 + upRight.y * -1
            + left.y * 0 + center.y * 0 + right.y * 0
            + downLeft.y * 1 + down.y * 2 + downRight.y * 1;
    
    float sobelYg = upLeft.y * -1 + up.y * 0 + upRight.y * 1
            + left.y * -2 + center.y * 0 + right.y * 2
            + downLeft.y * -1 + down.y * 0 + downRight.y * 1;

    float sobelXb = upLeft.z * -1 + up.z * -2 + upRight.z * -1
            + left.z * 0 + center.z * 0 + right.z * 0
            + downLeft.z * 1 + down.z * 2 + downRight.z * 1;
    
    float sobelYb = upLeft.z * -1 + up.z * 0 + upRight.z * 1
            + left.z * -2 + center.z * 0 + right.z * 2
            + downLeft.z * -1 + down.z * 0 + downRight.z * 1;

    float sobelR = sqrt((sobelXr * sobelXr) + (sobelYr * sobelYr));
    float sobelG = sqrt((sobelXg * sobelXg) + (sobelYg * sobelYg));
    float sobelB = sqrt((sobelXb * sobelXb) + (sobelYb * sobelYb));

    sobel = (sobelR + sobelG + sobelB) / 3;
}

void sobelDepth_float(float center, float upLeft, float up, float upRight, float left, float right, float downLeft, float down, float downRight, out float sobel)
{
    float sobelX = upLeft * -1 + up * -2 + upRight * -1
            + left * 0 + center * 0 + right * 0
            + downLeft * 1 + down * 2 + downRight * 1;

    float sobelY = upLeft * -1 + up * 0 + upRight * 1
            + left * -2 + center * 0 + right * 2
            + downLeft * -1 + down * 0 + downRight * 1;

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