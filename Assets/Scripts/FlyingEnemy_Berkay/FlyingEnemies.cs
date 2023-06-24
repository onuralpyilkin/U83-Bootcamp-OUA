using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float detectionDistance = 3f; // algi mesafesi (eger player'a 3 metre yakınsa kendinden 2 tane daha klonlar)
    public float copyOffset = 20f; // copyalama yaricapi

    [Header("Movement Settings")]
    public float movementSpeed = 3f;
    public float suzulmeYuksekligi = 10f;
    public float suzulmeHizi = 2f;
    public float idleDuration = 2f;
    public float collisionBekleme = 2f;

    [Header("Fog Settings")]
    public float fogDensityAlive = 0.2f;
    public float fogDensityDead = 0f;
    public Color fogColor;
    public Color defaultFogColor;

    
    private Vector3 initialPosition;
    private bool _Suzulme = false;
    private bool isAttacking = false;
    private bool inCollided = false;
    private Rigidbody _rb;
    private Animator _animator;

    private bool isPlayerDetected = false;
    private bool hasCopied = false;
    private GameObject[] copyEnemies;
    private Transform playerBodyTransform;
    private bool isDead = false;

    [Header("Attack Settings")]
    [SerializeField]private float attackInterval = 2f; // Saldırı aralığı (her 2 saniyede bir)
    private float nextAttackTime = 0f;
    public int damageCount = 2;
    public string attackPlayerTag = "PlayerBody";


    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // Duvarlardan gecmeyi ac
    }


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        initialPosition = transform.position;

        GameObject playerBodyObject = GameObject.FindGameObjectWithTag(attackPlayerTag);
        if (playerBodyObject != null)
        {
            playerBodyTransform = playerBodyObject.transform;
            isPlayerDetected = true;
        }
        else
        {
            Debug.LogError("PlayerBody object not found!");
        }

        
        StartAttacking();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (playerBodyTransform != null)
        {
            if(isAttacking)
            {
            
                // Karaktere doğru hareket
                Vector3 direction = playerBodyTransform.position - transform.position;
                direction.Normalize();
                transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);// FlyingEnemy hareketi

                //yüksekliği 1f olarak sınırla
                Vector3 newPosition = transform.position + (direction * movementSpeed * Time.deltaTime);
                newPosition.y = Mathf.Max(newPosition.y, 1.1f);

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

                // Yükselmeye başlama kontrolü
                if (Vector3.Distance(transform.position, playerBodyTransform.position) <= 1f)
                {
                    Invoke("StartAscending", collisionBekleme);
                }
            }
            else if (_Suzulme)
            {
                // Yükselme hareketi
                Vector3 targetPosition = new Vector3(initialPosition.x, suzulmeYuksekligi, initialPosition.z);
                Vector3 ascendDirection = targetPosition - transform.position;
                ascendDirection.Normalize();
                transform.Translate(ascendDirection * suzulmeHizi * Time.deltaTime, Space.World);

                Quaternion targetRotation = Quaternion.LookRotation(ascendDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
                {
                    _Suzulme = false;
                    Invoke("StartAttacking", idleDuration);
                }
            }
        }
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

    private void StartAttacking()
    {
        if (!inCollided)
        {
            _Suzulme = false;
            isAttacking = true;
        }
        else
        {
            Invoke("StartAttacking", collisionBekleme);
            inCollided = false;
        }
    }

    private void StartAscending()
    {
        _Suzulme = true;
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(attackPlayerTag))
        {
            inCollided = true;
            Invoke("ResetCollision", collisionBekleme);
        }
    }

    private void ResetCollision()
    {
        inCollided = false;
    }


    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(attackPlayerTag) && Time.time >= nextAttackTime)
        {
            // Oyuncuya 5 hasar verme
            PlayerController playerHealth = other.GetComponentInParent<PlayerController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageCount); //Player a hasar verme
                // Debug.Log("Mevcut Can :" + playerHealth.health);
                Debug.Log("Hasar verdi :" + damageCount);
            }
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(attackPlayerTag))
        {
            _rb.isKinematic = false;
            _animator.SetBool("isAttack", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag(attackPlayerTag))
        {
            _rb.isKinematic = true;
            _animator.SetBool("isAttack", false);
        }
    }
}
