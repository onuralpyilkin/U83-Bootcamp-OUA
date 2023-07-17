using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class PowerCubeController : MonoBehaviour
{
    public int PowerChargeSpeed = 1;
    public int PowerMaxAmount = 100;
    private float powerAmount;
    private Material material;
    private float emissionStartValue;
    private VisualEffect vfx;
    private GameObject vfxGameObject;
    public UnityEvent OnCubeOutOffCharge;
    void Start()
    {
        material = GetComponent<Renderer>().material;
        powerAmount = PowerMaxAmount;
        emissionStartValue = material.GetFloat("_EmissionIntensity");
        vfx = GetComponentInChildren<VisualEffect>();
        vfxGameObject = vfx.gameObject;
        vfxGameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vfxGameObject.SetActive(true);
            vfx.SetFloat("Lifetime", PowerMaxAmount / PowerChargeSpeed);
            vfx.Play();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerAmount <= 0)
            {
                vfx.Stop();
                vfxGameObject.SetActive(false);
                return;
            }
            powerAmount -= PowerChargeSpeed * Time.deltaTime;
            PlayerController.Instance.AddPower(PowerChargeSpeed * Time.deltaTime);
            material.SetFloat("_EmissionIntensity", emissionStartValue * (powerAmount / (float)PowerMaxAmount));
            if (powerAmount <= 0)
            {
                if (OnCubeOutOffCharge != null)
                    OnCubeOutOffCharge.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vfx.Stop();
            vfxGameObject.SetActive(false);
        }
    }
}
