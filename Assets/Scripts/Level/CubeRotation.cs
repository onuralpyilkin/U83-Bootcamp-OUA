using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float minRotationSpeed = 10f; // Minimum d�n�� h�z�
    public float maxRotationSpeed = 100f; // Maksimum d�n�� h�z�

    private float rotationSpeedX; // X ekseni d�n�� h�z�
    private float rotationSpeedY; // Y ekseni d�n�� h�z�
    private float rotationSpeedZ; // Z ekseni d�n�� h�z�

    private void Start()
    {
        // Rastgele d�n�� h�zlar�n� ayarla
        rotationSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        // K�p� rastgele h�zlarda ve rastgele y�nlere d�nd�r
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }
}
