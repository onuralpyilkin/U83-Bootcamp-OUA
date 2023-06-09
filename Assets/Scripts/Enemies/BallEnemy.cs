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
    [SerializeField]
    public int Health { get; set; }

    public float DetectionRange = 10f;
    public float AttackRange = 2f;
    public float AttackSpeed = 1f; //the time between attacks

    public int Damage = 1;
    private NavMeshAgent agent;
    private Animator animator;
    private int openTriggerHash, closeTriggerHash;
    private Transform childBody;
    public float ChildBodyRadius = 0.5f;
    private float bodyCircumference;
    private State state;
    public float AttackRotationSpeed = 5f;
    public float AttackStartDelay = 0.5f;
    private float attackStartTimer = 0f;
    private float attackTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        openTriggerHash = Animator.StringToHash("Open");
        closeTriggerHash = Animator.StringToHash("Close");
        childBody = transform.GetChild(0);
        bodyCircumference = ChildBodyRadius * Mathf.PI * 2;
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y = 0;
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        playerPosition.y = 0;
        float distance = Vector3.Distance(position, playerPosition);
        if (distance < DetectionRange)
        {
            state = State.Chasing;
            agent.SetDestination(PlayerController.Instance.transform.position);
            if (distance < AttackRange)
            {
                state = State.Attacking;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closed") || animator.GetCurrentAnimatorStateInfo(0).IsName("Closing"))
                {
                    animator.ResetTrigger(closeTriggerHash);
                    animator.SetTrigger(openTriggerHash);
                }
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
        //Debug.Log(state);
        if (state == State.Idle)
        {
            attackStartTimer = 0f;
            childBody.localEulerAngles = Vector3.Lerp(childBody.eulerAngles, Vector3.zero, Time.deltaTime * 10f);
        }
        else if (state == State.Chasing)
        {
            attackStartTimer = 0f;
            float currentVelocityForward = agent.velocity.magnitude * (transform.InverseTransformDirection(agent.velocity).z > 0 ? 1 : -1);
            float currentVelocityRight = agent.velocity.magnitude * (transform.InverseTransformDirection(agent.velocity).x > 0 ? 1 : -1);
            childBody.Rotate(360 * (currentVelocityRight / bodyCircumference) * Time.deltaTime, 0, 360 * (currentVelocityForward / bodyCircumference) * Time.deltaTime);
        }
        else if (state == State.Attacking)
        {
            if(attackStartTimer < AttackStartDelay)
            {
                attackStartTimer += Time.deltaTime;
                attackTimer = 0f;
                return;
            }
            attackTimer += Time.deltaTime;
            if(attackTimer >= AttackSpeed)
            {
                attackTimer = 0f;
                PlayerController.Instance.TakeDamage(Damage);
            }
            childBody.localEulerAngles = Vector3.Lerp(childBody.eulerAngles, new Vector3(0, childBody.eulerAngles.y + AttackRotationSpeed, 0), Time.deltaTime * AttackRotationSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        //Debug.Log("Enemy took " + damage + " damage. Health: " + Health);
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died.");
        state = State.Dead;
        Destroy(gameObject);
    }
}
