using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyClones : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]private GameObject vfxPrefab;
    public float movementSpeed = 3f;
    public float suzulmeYuksekligi = 10f;
    public float suzulmeHizi = 5f;
    public float idleDuration = 2f;
    public float collisionBekleme = 2f;

    private Transform playerBodyTransform;
    private Vector3 initialPosition;
    private bool _Suzulme = false;
    private bool isAttacking = false;
    private bool inCollided = false;
    private GameObject vfxInstance;
    private Rigidbody _rb;
    private Animator _animator;

    
    [Header("Attack Settings")]
    [SerializeField]private float attackInterval = 2f; // Saldırı aralığı (her 2 saniyede bir)
    private float nextAttackTime = 0f;
    public int damageCount = 5;
    public string attackPlayerTag = "PlayerBody";


    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // Duvarlardan gecmeyi ac
        _animator = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        initialPosition = transform.position;

        GameObject playerBodyObject = GameObject.FindGameObjectWithTag("PlayerBody");
        if (playerBodyObject != null)
        {
            playerBodyTransform = playerBodyObject.transform;
        }
        else
        {
            Debug.LogError("PlayerBody object not found!");
        }

        if (vfxPrefab != null)
        {
            vfxInstance = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            vfxInstance.transform.SetParent(transform);
            StartCoroutine(DestroyVFXAfterDelay());
        }

        StartAttacking();
    }

    private void Update()
    {
        if (playerBodyTransform != null)
        {
            if (isAttacking)
            {
                // Karaktere doğru hareket
                Vector3 direction = playerBodyTransform.position - transform.position;
                direction.Normalize();
                transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

                //yüksekliği 1f olarak sınırla
                Vector3 newPosition = transform.position + (direction * movementSpeed * Time.deltaTime);
                newPosition.y = Mathf.Max(newPosition.y, 1.1f);

                transform.position = newPosition; //dusman hareketi

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

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
        if (collision.gameObject.CompareTag("PlayerBody"))
        {
            inCollided = true;
            Invoke("ResetCollision", collisionBekleme);
        }
    }

    private void ResetCollision()
    {
        inCollided = false;
    }

    private IEnumerator DestroyVFXAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (vfxInstance != null)
        {
            Destroy(vfxInstance);
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