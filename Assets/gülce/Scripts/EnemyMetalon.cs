using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMetalon : MonoBehaviour
{
    [Header ("EnemyMetalon Health")]
    public int health = 100;

    private int damageAmount;

    public Animator enemyMetalonAnim;

    private void Start()
    {
        enemyMetalonAnim = GetComponent<Animator>();
    }

    // EnemyMetalon takes damage from player
    public void TakeDamage()   // Take Damage i player attack scriptinde cagir
    {
        health -= damageAmount;
        if(health <= 0)
        {
            // Death Anim
            enemyMetalonAnim.SetTrigger("die");
        }
        else
        {
            // Get Hit Anim
            enemyMetalonAnim.SetTrigger("damage");

        }
    }

    public void DamageToPlayer()
    {
        // player ile çarpıştığında player health i azalt
        
    }
    
}
