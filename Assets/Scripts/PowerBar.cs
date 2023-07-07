using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI powerText;
    private PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.Instance;
        SetMaxPower(playerController.MaxPower);
        SetPower(0);
    }

    public void SetMaxPower(float power)
    {
        slider.maxValue = power;
        slider.value = power;
    }
    public void SetPower(float power)
    {
        slider.value = power;
        UpdatePowerText(power);
    }

    private void UpdatePowerText(float power)
    {
        powerText.text = "" + power.ToString();
    }
}
