using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEndBehaviour : StateMachineBehaviour
{
    private bool isStarted = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isStarted = true;
        if (PlayerController.Instance.ComboExpireTimer != null)
        {
            PlayerController.Instance.StopCoroutine(PlayerController.Instance.ComboExpireTimer);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.99f && isStarted)
        {
            PlayerInputManager.Instance.AttackStarted = false;
            PlayerController.Instance.ComboExpireTimer = PlayerController.Instance.StartCoroutine(PlayerController.Instance.ComboExpireTimerCoroutine());
            isStarted = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*PlayerInputManager.Instance.attackStarted = false;
        PlayerController.Instance.comboExpireTimer = PlayerController.Instance.StartCoroutine(PlayerController.Instance.ComboExpireTimer());*/
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
