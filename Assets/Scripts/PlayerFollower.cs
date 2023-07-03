using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private Transform player;
    public float FollowSpeed = 5f;
    public float FollowMaxDistance = 20f;
    void Start()
    {
        player = PlayerController.Instance.transform;
        transform.position = player.position;
    }


    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        float speed = FollowSpeed * (dist / FollowMaxDistance);
        transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);
    }
}
