using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallEnemy : MonoBehaviour, IEnemy
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking,
        Dead
    }
    public int Health { get; set; }

    public float detectionRange = 10f;
    public float attackRange = 2f;

    //public int damage = 1;
    private NavMeshAgent agent;
    private Animator animator;
    private int openTriggerHash, closeTriggerHash;
    private Transform childBody;
    public float childBodyRadius = 0.5f;
    private State state;
    public float attackRotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        openTriggerHash = Animator.StringToHash("Open");
        closeTriggerHash = Animator.StringToHash("Close");
        childBody = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y = 0;
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        playerPosition.y = 0;
        float distance = Vector3.Distance(position, playerPosition);
        if (distance < detectionRange)
        {
            state = State.Chasing;
            agent.SetDestination(PlayerController.Instance.transform.position);
            if (distance < attackRange)
            {
                state = State.Attacking;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closed") || animator.GetCurrentAnimatorStateInfo(0).IsName("Closing"))
                {
                    animator.ResetTrigger(closeTriggerHash);
                    animator.SetTrigger(openTriggerHash);
                }
                //PlayerController.Instance.TakeDamage(damage);
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened") || animator.GetCurrentAnimatorStateInfo(0).IsName("Opening"))
                {
                    animator.ResetTrigger(openTriggerHash);
                    animator.SetTrigger(closeTriggerHash);
                }
            }
        }
        else
        {
            state = State.Idle;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened") || animator.GetCurrentAnimatorStateInfo(0).IsName("Opening"))
            {
                animator.ResetTrigger(openTriggerHash);
                animator.SetTrigger(closeTriggerHash);
            }
            agent.SetDestination(transform.position);
        }
        Debug.Log(state);
        if (state == State.Idle)
        {
            childBody.localEulerAngles = Vector3.Lerp(childBody.eulerAngles, Vector3.zero, Time.deltaTime * 10f);
        }
        else if (state == State.Chasing)
        {
            float currentVelocityForward = agent.velocity.magnitude * (transform.InverseTransformDirection(agent.velocity).z > 0 ? 1 : -1);
            float currentVelocityRight = agent.velocity.magnitude * (transform.InverseTransformDirection(agent.velocity).x > 0 ? 1 : -1);
            childBody.Rotate(360 * (currentVelocityRight / childBodyRadius) * Time.deltaTime, 0, 360 * (currentVelocityForward / childBodyRadius) * Time.deltaTime);
        }
        else if (state == State.Attacking)
        {
            childBody.localEulerAngles = Vector3.Lerp(childBody.eulerAngles, new Vector3(0, childBody.eulerAngles.y + attackRotationSpeed, 0), Time.deltaTime * attackRotationSpeed);
        }

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        state = State.Dead;
        Destroy(gameObject);
    }
}
