using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float detectionDistance = 3f; // algi mesafesi (eger player'a 3 metre yakınsa kendinden 2 tane daha klonlar)
    public float copyOffset = 10f; // copyalama yaricapi

    [Header("Movement")]
    public float movementSpeed = 3f;

    [Header("Fog Settings")]
    public float fogDensityAlive = 0.3f;
    public float fogDensityDead = 0f;
    public Color fogColor;
    public Color defaultFogColor;

    private bool isPlayerDetected = false;
    private bool hasCopied = false;
    private GameObject[] copyEnemies;
    private Transform playerBodyTransform;
    private bool isDead = false;

    private Rigidbody _rb;

    public bool colliderEnter = false;
    private float freezeDuration = 0.2f;

    [Header("Attack Settings")]
    [SerializeField]private float attackInterval = 2f; // Saldırı aralığı (her 2 saniyede bir)
    private float nextAttackTime = 0f;
    public int damageCount = 5;
    public string attackPlayerTag = "PlayerBody";

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.None; //Freeze position devre disi bırak
        GameObject playerBodyObject = GameObject.FindGameObjectWithTag("PlayerBody");
        if (playerBodyObject != null)
        {
            playerBodyTransform = playerBodyObject.transform;
            isPlayerDetected = true;
        }
        else
        {
            Debug.LogError("PlayerBody object not found!");
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isPlayerDetected && playerBodyTransform != null)
        {
            Vector3 direction = playerBodyTransform.position - transform.position;
            direction.Normalize();
            // transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World); // FlyingEnemy hareketi

            //yüksekliği 1f olarak sınırla
            Vector3 newPosition = transform.position + (direction * movementSpeed * Time.deltaTime);
            newPosition.y = Mathf.Max(newPosition.y, 1f);

            transform.position = newPosition; //dusman hareketi

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            float distanceToPlayer = Vector3.Distance(transform.position, playerBodyTransform.position); //FlyingEnemy ile oyuncu arasindaki mesafe
            if (distanceToPlayer <= detectionDistance && !hasCopied)
            {
                hasCopied = true;
                SpawnEnemies();
                ActivateFog(true);
            }
        }

        if(colliderEnter == false)
        {
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; // Freeze position aktivasyonu

        }else
        Invoke("Unfreeze", freezeDuration);
    }

    private void SpawnEnemies()
    {
        copyEnemies = new GameObject[2];
        for (int i = 0; i < copyEnemies.Length; i++)
        {
            Vector3 copyOffsetVector = new Vector3(Random.Range(-copyOffset, copyOffset), 0f, Random.Range(-copyOffset, copyOffset));
            Vector3 spawnPosition = playerBodyTransform.position + copyOffsetVector;
            copyEnemies[i] = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void Kill()
    {
        if (copyEnemies != null)
        {
            foreach (GameObject copyEnemy in copyEnemies)
            {
                Destroy(copyEnemy);
            }
        }

        isDead = true;
        ActivateFog(false);
        Destroy(gameObject);
    }

    private void ActivateFog(bool activate)
    {
        RenderSettings.fog = activate; //fogu etkinlestir - devre disi bırak
        if (activate)
        {
            RenderSettings.fogDensity = fogDensityAlive; // 3metre yakinsa fog yogunlugu
            RenderSettings.fogColor = fogColor; //fog rengi
        }
        else
        {
            RenderSettings.fogColor = defaultFogColor; //default fog rengi
            RenderSettings.fogDensity = fogDensityDead; // FlyingEnemy olurse fog yogunlugu
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(attackPlayerTag) && Time.time >= nextAttackTime)
        {
            // Oyuncuya 5 hasar verme
            PlayerTestHealth playerHealth = other.GetComponent<PlayerTestHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageCount); //Player a hasar verme
            }
            nextAttackTime = Time.time + attackInterval;
        }
        if(other.CompareTag("PlayerBody"))
        {colliderEnter = true;}
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayerBody"))
        {colliderEnter = false;}
    }

    private void Unfreeze()
    {
        _rb.constraints = RigidbodyConstraints.None; // Freeze position devre disi bırak
    }
}
