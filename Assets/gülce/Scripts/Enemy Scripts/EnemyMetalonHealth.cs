using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMetalonHealth : MonoBehaviour, IEnemy
{
    //EnemyMetalonHealth
    public int Health { get; set; }
    public bool HasTicket { get; set; }
    public EnemyGroupController GroupController { get; set; }
    public int maxHealth = 100;
    public int AttackDamage = 5;
    public float AttackRange = 1.5f;
    public LayerMask PlayerLayer;

    private Animator enemyMetalonAnim;

    private void Start()
    {
        Health = maxHealth;
        enemyMetalonAnim = GetComponent<Animator>();
    }

    // EnemyMetalon takes damage from player
    public void TakeDamage(int damage)   // Take Damage i player attack scriptinde cagir
    {
        Health -= damage;

        if(Health <= 0)
        {
            Die();
        }
        else
        {
           enemyMetalonAnim.SetTrigger("damage"); // EnemyMetalon Get Hit Anim
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died.");
        GroupController.RemoveEnemy(this);
        GroupController.PlayDieVFX(transform, 5f);
        enemyMetalonAnim.SetTrigger("die");  // EnemyMetalon Death Anim
        Destroy(gameObject);
    }

    public void Attack()
    {
        //Empty, just created to implement IEnemy interface, attack is handled in animation behaviour script
    }

    public bool CheckPlayerInAttackRange(float range, bool useDotProduct = true)
    {
        Collider[] player = Physics.OverlapSphere(transform.position, range, PlayerLayer);
        if (player.Length <= 0)
            return false;

        if (!useDotProduct)
            return true;

        float dotProduct = Vector3.Dot(transform.forward, (player[0].transform.position - transform.position).normalized);
        if (dotProduct > 0)
        {
            return true;
        }
        return false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
