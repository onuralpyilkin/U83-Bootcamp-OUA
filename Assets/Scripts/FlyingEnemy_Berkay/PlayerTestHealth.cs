using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestHealth : MonoBehaviour
{
    public int maxHealth = 100; // Toplam can
    private int currentHealth; // Mevcut can

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Can miktarından hasarı çıkar

        // Eğer canı <= 0 karakteri öldür
        if (currentHealth <= 0)
        {
            Debug.Log("Player öldü!");
        }
    }
}