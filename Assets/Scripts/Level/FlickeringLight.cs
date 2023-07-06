using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.5f;        
    public float maxIntensity = 1.5f;          
    public float minDelay = 0.1f;              
    public float maxDelay = 0.5f;              

    private Light torchLight;                    
    private float targetIntensity;                
    private float currentIntensity;               
    private float changeDelay;                    
    private float timer;                          

    void Start()
    {
        torchLight = GetComponent<Light>();
        currentIntensity = torchLight.intensity;
        SetNewTargetIntensity();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            
            SetNewTargetIntensity();

            
            changeDelay = Random.Range(minDelay, maxDelay);
            timer = changeDelay;
        }

        // transition
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime / changeDelay);

        
        torchLight.intensity = currentIntensity;
    }

    void SetNewTargetIntensity()
    {
        
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }
}
