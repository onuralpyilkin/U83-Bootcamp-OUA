using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
    float timer;
    List<Vector3> wayPoints = new List<Vector3>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 7.0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        timer = 0;
        GameObject gameObject = GameObject.FindGameObjectWithTag("WayPoints");
        if (gameObject != null)
        {
            foreach (Transform transform in gameObject.transform)
                wayPoints.Add(transform.position);

            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)]);
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                wayPoints.Add(animator.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
            }
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)]);

        timer += Time.deltaTime;
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        float distance = UnityEngine.Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
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
