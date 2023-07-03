using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    private Transform player;
    private new Rigidbody rigidbody;
    public float moveSpeed = 5f;
    public float enemyDistance = 2f;
    public GameObject attackPrefab;
    public float attackDelay = 1f;
    public float destroyDelay = 1f;
    private float nextAttackTime;
    public float attackHeight = 3f;
   



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Düşmanın oyuncuya olan mesafesini kontrol et
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Düşmanı oyuncunun konumuna doğru hareket ettir
        Vector3 moveDirection = (player.position - transform.position).normalized;
        if (distanceToPlayer > enemyDistance)
        {
            rigidbody.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidbody.velocity = Vector3.zero;

            // Saldırı yapma kontrolleri
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackDelay;
                PerformAttack();
            }
        }

        // Düşmanı oyuncuya doğru döndür
        transform.LookAt(player);
    }

    private void PerformAttack()
    {
        // Saldırı pozisyonunu hesapla
        Vector3 attackPosition = player.position + Vector3.up * attackHeight;

        // Saldırı prefabını oluştur ve saldırı pozisyonuna yerleştir
        GameObject attack = Instantiate(attackPrefab, attackPosition, Quaternion.identity);

        // Belirli bir süre sonra saldırıyı yok et
        Destroy(attack, attackDelay);
       
       

    }
}

