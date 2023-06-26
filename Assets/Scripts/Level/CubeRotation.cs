using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float minRotationSpeed = 10f; // Minimum dönüþ hýzý
    public float maxRotationSpeed = 100f; // Maksimum dönüþ hýzý

    private float rotationSpeedX; // X ekseni dönüþ hýzý
    private float rotationSpeedY; // Y ekseni dönüþ hýzý
    private float rotationSpeedZ; // Z ekseni dönüþ hýzý

    private void Start()
    {
        // Rastgele dönüþ hýzlarýný ayarla
        rotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        // Küpü rastgele hýzlarda ve rastgele yönlere döndür
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }
}
