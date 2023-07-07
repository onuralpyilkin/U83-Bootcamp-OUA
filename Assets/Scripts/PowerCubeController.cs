using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCubeController : MonoBehaviour
{
    public int PowerChargeSpeed = 1;
    public int PowerMaxAmount = 100;
    private float powerAmount;
    private Material material;
    private float emissionStartValue;
    void Start()
    {
        material = GetComponent<Renderer>().material;
        powerAmount = PowerMaxAmount;
        emissionStartValue = material.GetFloat("_EmissionIntensity");
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(powerAmount <= 0)
            {
                return;
            }
            powerAmount -= PowerChargeSpeed * Time.deltaTime;
            PlayerController.Instance.AddPower(PowerChargeSpeed * Time.deltaTime);
            material.SetFloat("_EmissionIntensity", emissionStartValue * (powerAmount / (float)PowerMaxAmount));
        }
    }
}
