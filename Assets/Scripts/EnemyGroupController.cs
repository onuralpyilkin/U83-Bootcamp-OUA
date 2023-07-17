using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGroupController : MonoBehaviour
{
    public VFXPoolController DieVFXPool;
    public float TicketChangeRate = 30f;
    private float ticketChangeTimer = 30f;
    private int ticketOwnerIndex = -1;
    private int lastTicketOwnerIndex = -1;
    List<IEnemy> enemies = new List<IEnemy>();
    public UnityEvent OnEnemyGroupEnable;
    public UnityEvent OnEnemyGroupDie;
    void Start()
    {
        enemies = new List<IEnemy>(GetComponentsInChildren<IEnemy>());
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GroupController = this;
        }
        ticketChangeTimer = TicketChangeRate + 1f;
        if (OnEnemyGroupEnable != null)
            OnEnemyGroupEnable.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0)
        {
            if (ticketChangeTimer >= TicketChangeRate)
            {
                PickTicketOwner();
            }
            else
            {
                ticketChangeTimer += Time.deltaTime;
            }
        }
        else
        {
            if (OnEnemyGroupDie != null)
                OnEnemyGroupDie.Invoke();
            Destroy(gameObject);
        }
    }

    public void RemoveEnemy(IEnemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count > 0)
        {
            ticketOwnerIndex = 0;
            PickTicketOwner();
        }
    }

    void PickTicketOwner()
    {
        if(enemies.Count == 1){
            enemies[0].HasTicket = true;
            return;
        }
        ticketChangeTimer = 0f;
        if (ticketOwnerIndex >= 0)
        {
            enemies[ticketOwnerIndex].HasTicket = false;
        }
        ticketOwnerIndex = Random.Range(0, enemies.Count);
        if(ticketOwnerIndex == lastTicketOwnerIndex)
            ticketOwnerIndex = (ticketOwnerIndex + 1) % enemies.Count;
        enemies[ticketOwnerIndex].HasTicket = true;
        lastTicketOwnerIndex = ticketOwnerIndex;
    }

    public void PlayDieVFX(Transform transform, float DieVFXLifeTime, float offsetY = 1f)
    {
        DieVFXPool.SelfRelease = true;
        DieVFXPool.ReleaseDelay = DieVFXLifeTime;
        VFX vfx = DieVFXPool.Get();
        vfx.SetPosition(transform.position + Vector3.up * offsetY);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        //StartCoroutine(ReleaseDieVFX(vfx, DieVFXLifeTime));
    }

    IEnumerator ReleaseDieVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        //DashVFXPool.Release(vfx);
        DieVFXPool.Release(vfx);
        yield break;
    }
}
