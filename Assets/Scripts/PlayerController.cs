using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Player Stats")]
    private int health = 100;
    public int MaxHealth = 100;

    // HealthBar referansı
    public HealthBar healthBar;

    [Header("Movement Animation Thresholds")]
    public float WalkThreshold = 0.1f;
    public float RunThreshold = 0.9f;
    public float CombatWalkThreshold = 0.1f;

    [Header("Movement Values")]
    public float Acceleration = 1f;
    public float Deceleration = 1f;
    public float TurnSpeed = 1f;

    private float velocity, targetVelocity;
    private bool isRunning;
    private float angle, targetAngle; //angles on the Y axis
    private Vector2 moveDirection;

    //Animator variables
    private Animator animator;
    private int velocityHash;

    [Header("Combos")]
    public Combo[] Combos;
    private int currentComboIndex = 0;
    private int currentAttackIndex = 0;
    public float ComboExpireTime = 0.5f;
    [HideInInspector]
    public Coroutine ComboExpireTimer;
    private bool isComboLayerActive = false;
    public float ComboLayerWeightChangeSpeed = 5f;
    public float AnimationSpeed = 1f;
    private int animationSpeedHash;
    [HideInInspector]
    public bool IsAttacking = false;
    [HideInInspector]
    public float ComboExpireStartTime = 0f;
    private bool isComboActive = false;
    public LayerMask EnemyLayer;

    [Header("Dash")]
    public float DashDistance = 5;
    public float DashCooldown = 1;
    public bool DashAvailable = true;


    [Header("Dash VFX")]
    public VFXPoolController DashVFXPool;
    public float DashVFXLifeTime = 1;
    public float DashVFXParticleCount = 100;
    [ColorUsageAttribute(true, true)]
    public Color DashVFXParticleColor = Color.white;

    //Other Components
    private CameraController cameraController;
    private PlayerInputManager inputManager;
    private AudioSource swordAudioSource;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        cameraController = CameraController.Instance;
        inputManager = PlayerInputManager.Instance;
        swordAudioSource = GetComponentInChildren<AudioSource>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        animationSpeedHash = Animator.StringToHash("AnimationSpeed");
        animator.SetFloat(animationSpeedHash, AnimationSpeed);
        //healthBar.SetMaxHealth(MaxHealth);


        for (int i = 0; i < Combos.Length; i++)
        {
            Combos[i].Initialize();
        }
        int count = DashVFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = DashVFXPool.Get();
            vfx.SetFloat("Lifetime", DashVFXLifeTime);
            vfx.SetFloat("ParticleCount", DashVFXParticleCount);
            vfx.SetVector4("Color", DashVFXParticleColor);
            DashVFXPool.Release(vfx);
        }
    }

    void Update()
    {
        //Set Velocity
        velocity = Mathf.MoveTowards(velocity, targetVelocity * (isComboLayerActive ? CombatWalkThreshold : (isRunning ? RunThreshold : WalkThreshold)), (velocity <= targetVelocity ? Acceleration : Deceleration) * Time.deltaTime);
        animator.SetFloat(velocityHash, velocity);

        //Set Y Angle
        if (velocity != 0 && moveDirection != Vector2.zero)
            targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cameraController.AngleOnYAxis;
        angle = Mathf.LerpAngle(angle, targetAngle, TurnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Debug
        float currentAnimLength = animator.GetCurrentAnimatorClipInfo(1).Length;
        float currentAnimTime = animator.GetCurrentAnimatorStateInfo(1).normalizedTime * currentAnimLength;

        animator.SetLayerWeight(1, Mathf.MoveTowards(animator.GetLayerWeight(1), isComboLayerActive ? 1 : 0, Time.deltaTime * ComboLayerWeightChangeSpeed));
    }

    public void SetTargetVelocity(float velocity)
    {
        targetVelocity = velocity;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    public void SetRunState(bool state)
    {
        isRunning = state;
    }

    public T[] ShuffleArray<T>(T[] array)
    {
        //fisher-yates shuffle
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            T temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }
        return array;
    }

    public void Attack()
    {
        if (currentAttackIndex + 1 > Combos[currentComboIndex].GetComboLength() || !isComboActive)
        {
            currentAttackIndex = 0;
        }

        if (currentAttackIndex == 0)
        {
            currentComboIndex = Random.Range(0, Combos.Length);
            //comboTimer = StartCoroutine(ComboExpireTimer());
        }

        isComboLayerActive = true;
        isComboActive = true;
        IsAttacking = true;
        animator.SetTrigger(Combos[currentComboIndex].Attacks[currentAttackIndex].TriggerHash);
        currentAttackIndex++;

        List<IEnemy> enemies = GetEnemiesInAttackRange(Combos[currentComboIndex].Attacks[currentAttackIndex - 1].Range);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].TakeDamage((int)Combos[currentComboIndex].Attacks[currentAttackIndex - 1].Damage);
        }
    }

    public IEnumerator ComboExpireTimerCoroutine()
    {
        ComboExpireStartTime = Time.time;
        while (Time.time - ComboExpireStartTime < ComboExpireTime)
        {
            yield return null;
        }
        isComboActive = false;
        currentAttackIndex = 0;
        isComboLayerActive = false;
    }

    List<IEnemy> GetEnemiesInAttackRange(float range, bool useDotProduct = true)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, EnemyLayer);
        List<IEnemy> iEnemies = new List<IEnemy>();
        if (!useDotProduct)
            return iEnemies;
        for (int i = 0; i < enemies.Length; i++)
        {
            float dotProduct = Vector3.Dot(transform.forward, (enemies[i].transform.position - transform.position).normalized);
            if (dotProduct > 0)
            {
                IEnemy iEnemy = enemies[i].GetComponent<IEnemy>();
                if (iEnemy != null)
                    iEnemies.Add(iEnemy);
            }
        }
        return iEnemies;
    }

    public void Dash(Vector2 dir)
    {
        if (!DashAvailable || DashVFXPool.GetCount() == 0)
            return;
        StartCoroutine(DashCooldownCoroutine());
        Vector3 dashDirection = new Vector3(dir.x, 0, dir.y);
        PlayVFX();
        transform.position += dashDirection * DashDistance;
        PlayVFX();
    }

    IEnumerator DashCooldownCoroutine()
    {
        DashAvailable = false;
        yield return new WaitForSeconds(DashCooldown);
        DashAvailable = true;
        yield break;
    }

    void PlayVFX()
    {
        VFX vfx = DashVFXPool.Get();
        vfx.SetPosition(transform.position);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        StartCoroutine(ReleaseVFX(vfx, DashVFXLifeTime));
    }

    IEnumerator ReleaseVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        DashVFXPool.Release(vfx);
        yield break;
    }

    public void PlaySwordSound()
    {
        swordAudioSource.Play();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100); // 0 ile 100 arasında sınırlandır
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("You died.");
    }
}
