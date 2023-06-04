using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCapsule : MonoBehaviour
{
    public float health = 100f;
    public float playerFightDistance = 1f;
    public float playerDetectDistance = 10f;
    private float distanceToPlayer;
    public bool isInRange = false;
    void Start()
    {
        PlayerController.Instance.enemies.Add(this);
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        if (distanceToPlayer <= playerDetectDistance)
        {
            transform.LookAt(PlayerController.Instance.transform);
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }
}
