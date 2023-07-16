using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVFXOptimizer : MonoBehaviour
{
    public GameObject[] VFXTiles;
    public float MaxDistanceToOrigin = 200f;
    private Transform playerTransform;
    public float SizeOfMap = 100f; //100x100
    public float DeactivationDistance = 100f;

    void Start()
    {
        //VFXTiles = GetComponentsInChildren<GameObject>();
        playerTransform = PlayerController.Instance.transform;
    }

    private void Update()
    {
        for (int i = 0; i < VFXTiles.Length; i++)
        {
            Vector3 tilePos = VFXTiles[i].transform.position;
            Vector3 playerPos = playerTransform.position;

            float xDifference = playerPos.x - tilePos.x;
            float zDifference = playerPos.z - tilePos.z;

            if (xDifference > MaxDistanceToOrigin || xDifference < -MaxDistanceToOrigin)
            {
                VFXTiles[i].transform.position += new Vector3(SizeOfMap * 1.5f * ((int)(xDifference / SizeOfMap)), 0, 0);
            }

            if (zDifference > MaxDistanceToOrigin || zDifference < -MaxDistanceToOrigin)
            {
                VFXTiles[i].transform.position += new Vector3(0, 0, SizeOfMap * 1.5f * ((int)(zDifference / SizeOfMap)));
            }

            if (Vector3.Distance(tilePos, playerPos) < DeactivationDistance)
            {
                VFXTiles[i].SetActive(true);
            }
            else
            {
                VFXTiles[i].SetActive(false);
            }
        }
    }
}
