using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyClones : MonoBehaviour
{
    public GameObject vfxPrefab;

    [Header("Movement")]
    public float movementSpeed = 3f;

    private Transform playerBodyTransform;
    private GameObject vfxInstance;
    private Rigidbody _rb;

    private bool colliderEnter = false;
    private float freezeDuration = 0.2f;

    [Header("Attack Settings")]
    [SerializeField]private float attackInterval = 2f; // Saldırı aralığı (her 2 saniyede bir)
    private float nextAttackTime = 0f;
    public int damageCount = 5;
    public string attackPlayerTag = "PlayerBody";


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.None; // Freeze position devre disi bırak
        GameObject playerBodyObject = GameObject.FindGameObjectWithTag("PlayerBody");
        if (playerBodyObject != null)
        {
            playerBodyTransform = playerBodyObject.transform; //hedef oyuncu
        }
        else
        {
            Debug.LogError("PlayerBody object not found!");
        }

        if (vfxPrefab != null)
        {
            vfxInstance = Instantiate(vfxPrefab, transform.position, Quaternion.identity); //vfxPrefab objesini olustur
            vfxInstance.transform.SetParent(transform); //vfxPrefab objesini düşman objesinin altina ekle
            StartCoroutine(DestroyVFXAfterDelay()); //particle yok etmek icin gereken süre
        }
    }

    private void Update()
    {
        if (playerBodyTransform != null)
        {
            Vector3 direction = playerBodyTransform.position - transform.position; 
            direction.Normalize();

            // Yeni pozisyonu hesapla ve yüksekliği en az 1f olarak sınırla
            Vector3 newPosition = transform.position + (direction * movementSpeed * Time.deltaTime);
            newPosition.y = Mathf.Max(newPosition.y, 1f);

            transform.position = newPosition; // Dusman hareketi

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Dusmanin rotasyonu
        }

        if(colliderEnter == false)
        {
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; // Freeze position etkinlestirme

        }else
        Invoke("Unfreeze", freezeDuration);
    }

    private void OnDestroy()
    {
        if (vfxInstance != null)
        {
            Destroy(vfxInstance); //particle yok etme
        }
    }

    private IEnumerator DestroyVFXAfterDelay()
    {
        yield return new WaitForSeconds(2f); //particle yok etmek icicin gereken sure
        if (vfxInstance != null)
        {
            Destroy(vfxInstance); //particle yok etme
        }
    }


    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(attackPlayerTag) && Time.time >= nextAttackTime)
        {
            // Oyuncuya 5 hasar verme
            PlayerTestHealth playerHealth = other.GetComponent<PlayerTestHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageCount);
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