using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private Transform player;
    private bool hasDamagedPlayer = false;
    private EnemyMetalonHealth enemyMetalonHealth;
    private int attackDamage = 5;
    private float attackRange = 1.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.transform;
        enemyMetalonHealth = animator.GetComponent<EnemyMetalonHealth>();
        attackDamage = enemyMetalonHealth.AttackDamage;
        attackRange = enemyMetalonHealth.AttackRange;
        hasDamagedPlayer = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", true);
        animator.transform.LookAt(player);  //playera doğru bak
        /*float distance = Vector3.Distance(player.position, animator.transform.position); //player ve enemymetalon arasındaki mesafe
        if (distance > attackRange)
        {
            animator.SetBool("isAttacking", false);
            hasDamagedPlayer = false;
        }
        else if (distance <= attackRange && !hasDamagedPlayer) // Attack range
        {
            DealDamageToPlayer();
            if (!hasDamagedPlayer)
            {
                hasDamagedPlayer = false;
                animator.SetBool("isAttacking", false);
            }
        }*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DealDamageToPlayer();
        hasDamagedPlayer = false; //hasDamagedPlayerı sıfırla bir sonraki saldırı için hazırlan
    }

    private void DealDamageToPlayer()  // Playera zarar ver
    {   //player scriptine ulaş takedamage fonk çağır
        if (enemyMetalonHealth.CheckPlayerInAttackRange(attackRange, true))
        {
            PlayerController.Instance.TakeDamage(attackDamage);
        }
    }
}
