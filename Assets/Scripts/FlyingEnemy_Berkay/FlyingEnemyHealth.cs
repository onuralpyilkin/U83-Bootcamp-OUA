using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; // Toplam can
    private int currentHealth; // Mevcut can

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Düşmanın canını azaltan metod
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Can miktarından hasarı çıkar

        // Eğer canı <= 0 düşmanı öldür
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}