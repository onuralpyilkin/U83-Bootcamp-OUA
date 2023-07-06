using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int Health { get; set; }
    bool HasTicket { get; set; }
    EnemyGroupController GroupController { get; set; }

    void TakeDamage(int damage);

    void Die();
}
