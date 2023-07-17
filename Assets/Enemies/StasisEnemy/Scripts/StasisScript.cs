using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StasisScript : MonoBehaviour, IEnemy
{
    public int Health { get; set; }
    public bool HasTicket { get; set; }
    public EnemyGroupController GroupController { get; set; }

    public LayerMask PlayerLayer;
    public float AttackRange = 1f;
    public int AttackDamage = 5;
    Animator StasisAnim;

    Transform _player;

    NavMeshAgent _agent;

    private float _distance;
    public float _visionDistance = 6f;

    bool isTeleporting = false;
    bool isFleeing = false;

    void Start()
    {
        StasisAnim = GetComponent<Animator>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _player = PlayerController.Instance.transform;
        Health = 100;
    }

    void Update()
    {
        Vector3 lookDirection = _player.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;

        if (!HasTicket)
        {
            StopAllCoroutines();
            StasisAnim.SetBool("isTeleporting", false);
            StasisAnim.SetBool("fleeing", false);
            StasisAnim.SetFloat("speed", 0f);
            _agent.enabled = false;
            return;
        }
        StasisAnim.SetFloat("speed", _agent.velocity.magnitude);

        _distance = Vector3.Distance(transform.position, _player.position);

        if (/*_health*/ Health <= 5)
        {
            if (!isFleeing)
            {
                isFleeing = true;
                StartCoroutine(Flee());
            }
        }
        else
        {
            isFleeing = false;
            if (_distance <= 1.5f)
            {
                _agent.enabled = false;
                StasisAnim.SetBool("playerHere", true);
            }
            else
            {
                _agent.enabled = true;
                StasisAnim.SetBool("playerHere", false);
            }

            if (_agent.enabled)
            {
                if (_distance <= _visionDistance)
                {
                    _agent.destination = _player.position;
                }
                else
                {
                    _agent.destination = transform.position;
                }
            }
        }

        if (isTeleporting)
        {
            StartCoroutine(PlayIdleAnimation());
        }
    }

    IEnumerator Flee()
    {
        StasisAnim.SetBool("fleeing", true);
        StasisAnim.SetBool("isTeleporting", true);
        yield return new WaitForSeconds(2f);


        Vector3 fleePosition = FindFleePosition();
        transform.position = fleePosition;

        // can doldur
        /*_health = 50;*/
        Health = 100;


        _agent.enabled = false;

        yield return new WaitForSeconds(2f);

        // teleporttan sonra yeniden animasyon girsin
        StasisAnim.SetBool("fleeing", false);
        StasisAnim.SetBool("isTeleporting", false);


        _agent.enabled = true;
    }

    Vector3 FindFleePosition()
    {
        // en uzak noktay� bul
        Vector3 fleePosition = Vector3.zero;
        float maxDistance = 0f;

        NavMeshPath path = new NavMeshPath();
        NavMeshHit hit;

        for (int i = 0; i < 100; i++) // 100 deneme yap
        {
            Vector3 randomPosition = Random.insideUnitSphere * 20f; // Random konum se�me
            randomPosition += transform.position;
            randomPosition.y = transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas)) // bake var m� kontrol et
            {
                if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    float distance = GetPathDistance(path);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        fleePosition = hit.position;
                    }
                }
            }
        }

        return fleePosition;
    }

    float GetPathDistance(NavMeshPath path)
    {
        float distance = 0f;

        if (path.corners.Length < 2)
            return distance;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return distance;
    }

    IEnumerator PlayIdleAnimation()
    {
        yield return new WaitForSeconds(2f);

        isTeleporting = false;
        StasisAnim.SetBool("isTeleporting", false);
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
        Debug.Log("Enemy died.");
        //state = State.Dead;
        GroupController.RemoveEnemy(this);
        GroupController.PlayDieVFX(transform, 5f);
        Destroy(gameObject);
    }

    public void Attack()
    {
        if (CheckPlayerInAttackRange(AttackRange, true))
        {
            PlayerController.Instance.TakeDamage(AttackDamage);
        }
    }

    bool CheckPlayerInAttackRange(float range, bool useDotProduct = true)
    {
        Collider[] player = Physics.OverlapSphere(transform.position, range, PlayerLayer);
        if (player.Length <= 0)
            return false;

        if (!useDotProduct)
            return true;

        float dotProduct = Vector3.Dot(transform.forward, (player[0].transform.position - transform.position).normalized);
        if (dotProduct > 0)
        {
            return true;
        }
        return false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
