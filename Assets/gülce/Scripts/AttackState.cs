using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private Transform player;
    public int damageAmount = 10;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);  //playera doğru bak
        float distance = Vector3.Distance(player.position, animator.transform.position); //player ve enemymetalon arasındaki mesafe
        if (distance > 3.5f)
        {
            animator.SetBool("isAttacking", false);
        }
        else if (distance < 1.5f) // Attack range
        {
            //DealDamageToPlayer();   
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    //private void DealDamageToPlayer()  // Playera zarar ver
    //{

        //Player playerScript = player.GetComponent<Player>();   //player scriptine ulaş takedamage fonk çağır
        //if (playerScript != null)
        //{
        //    playerScript.TakeDamage(damageAmount);
        //}
    //}



}
