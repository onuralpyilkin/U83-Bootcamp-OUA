using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMetalon : MonoBehaviour
{
    //EnemyMetalonHealth
    public int maxHealth = 50;
    private int currentHealth;

    private int damageAmount; 

    public Animator enemyMetalonAnim;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyMetalonAnim = GetComponent<Animator>();
    }

    // EnemyMetalon takes damage from player
    public void TakeDamage()   // Take Damage i player attack scriptinde cagir
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            enemyMetalonAnim.SetTrigger("die");  // EnemyMetalon Death Anim
            Destroy(gameObject);
        }
        else
        {
           enemyMetalonAnim.SetTrigger("damage"); // EnemyMetalon Get Hit Anim
        }
    }

   // public void DamageToPlayer()   // player ile çarpıştığında player health i azalt
    //{
       
    //}
    
}
