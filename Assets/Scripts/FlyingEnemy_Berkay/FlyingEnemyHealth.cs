using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour, IEnemy
{
    public int Health { get; set; }
    public bool HasTicket { get; set; }
    public EnemyGroupController GroupController { get; set; }
    public int maxHealth = 30; // Toplam can
    //private int currentHealth; // Mevcut can
    public bool isClone = false;
    private VFXPoolController vfxPoolController;

    private void Start()
    {
        Health = maxHealth;
        if (!isClone)
            GroupController = GetComponentInParent<EnemyGroupController>();

        if (GroupController != null)
            vfxPoolController = GroupController.DieVFXPool;
    }

    // Düşmanın canını azaltan metod
    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount; // Can miktarından hasarı çıkar

        // Eğer canı <= 0 düşmanı öldür
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died.");
        if (!isClone)
            GroupController.RemoveEnemy(this);
        PlayDieVFX(transform, 5f, 0f);
        if (!isClone)
        {
            GetComponent<FlyingEnemies>().Kill();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        //Empty, just created to implement IEnemy interface, attack is handled in another script that is placed on the same object
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void PlayDieVFX(Transform transform, float DieVFXLifeTime, float offsetY = 1f)
    {
        vfxPoolController.SelfRelease = true;
        vfxPoolController.ReleaseDelay = DieVFXLifeTime;
        VFX vfx = vfxPoolController.Get();
        vfx.SetPosition(transform.position + Vector3.up * offsetY);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        //StartCoroutine(ReleaseDieVFX(vfx, DieVFXLifeTime));
    }
}