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
    private float powerAmount = 0;
    public float MaxPower = 100f;
    public int PowerAttackDamageMultiplier = 2;

    // HealthBar referansı
    public HealthBar healthBar;

    // PowerBar referansı
    public PowerBar powerBar;

    [Header("Movement Animation Thresholds")]
    public float WalkThreshold = 0.1f;
    public float RunThreshold = 0.9f;
    public float CombatWalkThreshold = 0.1f;

    [Header("Movement Values")]
    public float Acceleration = 1f;
    public float Deceleration = 1f;
    public float TurnSpeed = 1f;

    public SoundEffectPack WalkSoundEffectPack;
    public SoundEffectPack RunSoundEffectPack;

    private float velocity, targetVelocity;
    private bool isRunning;
    private float angle, targetAngle; //angles on the Y axis
    private Vector2 moveDirection;

    //Animator variables
    private Animator animator;
    private int velocityHash;

    [Header("Combos")]
    public comboUI ComboUI;
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
    public DashIndicator dashIndicator;
    public float DashDistance = 5;
    public float DashCooldown = 1;
    public float DashMoveTime = 0.1f;
    public bool DashAvailable = true;
    private int dashStartTriggerHash;
    private int dashEndTriggerHash;
    [HideInInspector]
    public bool dashStartStateRunning = false;
    [HideInInspector]
    public bool dashEndStateRunning = false;


    [Header("Dash VFX")]
    //public VFXPoolController DashVFXPool;
    public float DashVFXLifeTime = 1;
    //public float DashVFXParticleCount = 100;
    [ColorUsageAttribute(true, true)]
    public Color DashVFXParticleColor = Color.white;
    public VFXPoolController DashNewVFXPool;

    [Header("Sword VFX")]
    public VFXPoolController SwordVFXPool;
    public Transform SwordVFXSpawnReference;
    public float SwordVFXLifeTime = 1;
    public SoundEffectPack SwordSoundEffectPack;
    [ColorUsageAttribute(true, true)]
    public Color[] SwordVFXEmissionColors;
    private int lastSwordVFXEmissiveColorIndex = 0;
    public GameObject SwordTransform;
    public float SwordMaxEmission = 5f;
    private Material swordMaterial;

    [Header("Damage VFX")]
    public VFXPoolController DamageVFXPool;
    public float DamageVFXLifeTime = 3;
    private int lastDirectionOfDamageVFX = 1;

    //Other Components
    private CameraController cameraController;
    private PlayerInputManager inputManager;
    private AudioSource audioSource;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private int dodgeTriggerHash;
    private int idleTriggerHash;

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
        audioSource = GetComponentInChildren<AudioSource>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        animationSpeedHash = Animator.StringToHash("AnimationSpeed");
        dodgeTriggerHash = Animator.StringToHash("Dodge");
        idleTriggerHash = Animator.StringToHash("Idle");
        animator.SetFloat(animationSpeedHash, AnimationSpeed);
        //healthBar.SetMaxHealth(MaxHealth);


        for (int i = 0; i < Combos.Length; i++)
        {
            Combos[i].Initialize();
        }
        /*int count = DashVFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = DashVFXPool.Get();
            vfx.SetFloat("Lifetime", DashVFXLifeTime);
            vfx.SetFloat("ParticleCount", DashVFXParticleCount);
            vfx.SetVector4("Color", DashVFXParticleColor);
            DashVFXPool.Release(vfx);
        }*/

        int count = SwordVFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = SwordVFXPool.Get();
            vfx.SetFloat("Lifetime", SwordVFXLifeTime);
            SwordVFXPool.Release(vfx);
        }

        count = DashNewVFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = DashNewVFXPool.Get();
            vfx.SetFloat("Lifetime", DashVFXLifeTime);
            vfx.SetVector4("Color", DashVFXParticleColor);
            DashNewVFXPool.Release(vfx);
        }
        dashIndicator.UpdateDashIndicator(DashAvailable = true);
        dashStartTriggerHash = Animator.StringToHash("DashStart");
        dashEndTriggerHash = Animator.StringToHash("DashEnd");
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        swordMaterial = SwordTransform.GetComponent<MeshRenderer>().material;
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

    public void Dodge()
    {
        animator.SetTrigger(dodgeTriggerHash);
        isComboLayerActive = true;
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
        //ComboUI.comboSayac(); //Temporarily here for debugging, we will write it in where that enemies take damage (maybe inside the next for loop)
        DealDamage((int)Combos[currentComboIndex].Attacks[currentAttackIndex - 1].Damage * (powerAmount > 0 ? PowerAttackDamageMultiplier : 1), Combos[currentComboIndex].Attacks[currentAttackIndex - 1].Range, -Combos[currentComboIndex].Attacks[currentAttackIndex - 1].PowerCost);
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

    void DealDamage(int damage = 0, float range = 0, float powerCost = 0)
    {
        List<IEnemy> enemies = GetEnemiesInAttackRange(range);
        for (int i = 0; i < enemies.Count; i++)
        {
            PlayDamageVFX(damage);
            enemies[i].TakeDamage(damage);
            ComboUI.comboSayac();
        }
        AddPower(powerCost);
    }

    public void Dash(Vector2 dir)
    {
        /*if (!DashAvailable || DashVFXPool.GetCount() == 0)
            return;
        StartCoroutine(DashCooldownCoroutine());
        Vector3 dashDirection = new Vector3(dir.x, 0, dir.y);
        PlayDashVFX();
        transform.position += dashDirection * DashDistance;
        PlayDashVFX();*/

        StartCoroutine(DashCooldownCoroutine());
        isComboLayerActive = true;
        isComboActive = true;
        IsAttacking = true;
        Vector3 cameraForward = cameraController.BrainCamera.transform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = cameraController.BrainCamera.transform.right;
        Vector3 dashDirection = cameraForward * dir.y + cameraRight * dir.x;
        StartCoroutine(DashCoroutine(dashDirection.normalized, DashMoveTime));
    }

    IEnumerator DashCoroutine(Vector3 dashDirection, float dashTime = 0.1f)
    {
        dashStartStateRunning = true;
        animator.SetTrigger(dashStartTriggerHash);
        while (dashStartStateRunning)
        {
            yield return null;
        }
        skinnedMeshRenderer.enabled = false;
        SwordTransform.SetActive(false);
        PlayDashVFX();
        float startTime = Time.time;
        while (Time.time - startTime < dashTime)
        {
            yield return null;
        }
        transform.position += dashDirection * DashDistance;
        skinnedMeshRenderer.enabled = true;
        SwordTransform.SetActive(true);
        dashEndStateRunning = true;
        animator.SetTrigger(dashEndTriggerHash);
        DealDamage(10, 2, -10);
        while (dashEndStateRunning)
        {
            yield return null;
        }
        yield break;
    }

    IEnumerator DashCooldownCoroutine()
    {
        dashIndicator.UpdateDashIndicator(DashAvailable = false);
        yield return new WaitForSeconds(DashCooldown);
        dashIndicator.UpdateDashIndicator(DashAvailable = true);
        yield break;
    }

    void PlayDashVFX()
    {
        //VFX vfx = DashVFXPool.Get();
        VFX vfx = DashNewVFXPool.Get();
        vfx.SetPosition(transform.position + Vector3.up * 1f);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        StartCoroutine(ReleaseDashVFX(vfx, DashVFXLifeTime));
    }

    IEnumerator ReleaseDashVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        //DashVFXPool.Release(vfx);
        DashNewVFXPool.Release(vfx);
        yield break;
    }

    public void PlaySwordVFX(int isRightToLeft = 1)
    {
        VFX vfx = SwordVFXPool.Get();
        vfx.SetPosition(SwordVFXSpawnReference.position);
        vfx.SetRotation(SwordVFXSpawnReference.rotation);
        vfx.SetBool("IsRightToLeft", isRightToLeft == 1);
        if (SwordVFXEmissionColors.Length > 0)
        {
            int randomIndex = Random.Range(0, SwordVFXEmissionColors.Length);
            if (randomIndex == lastSwordVFXEmissiveColorIndex)
                randomIndex = (randomIndex + 1) % SwordVFXEmissionColors.Length;
            vfx.SetVector4("Emission Color", SwordVFXEmissionColors[randomIndex]);
            lastSwordVFXEmissiveColorIndex = randomIndex;
        }
        vfx.Play();
        StartCoroutine(ReleaseSwordVFX(vfx, SwordVFXLifeTime));
    }

    IEnumerator ReleaseSwordVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        SwordVFXPool.Release(vfx);
        yield break;
    }

    private void PlayDamageVFX(float damage)
    {
        VFX vfx = DamageVFXPool.Get();
        Vector3 spawnPosition = transform.position + Vector3.up * 1f;
        spawnPosition += transform.right * Random.Range(0.25f, 0.5f) * lastDirectionOfDamageVFX;
        lastDirectionOfDamageVFX *= -1;
        //spawnPosition += transform.forward * Random.Range(-0.5f, -0.1f);
        spawnPosition += transform.up * Random.Range(0.25f, 0.5f);
        int damageAmount = (int)damage;
        int digit1 = damageAmount / 10;
        int digit2 = damageAmount % 10;
        vfx.SetPosition(transform.position);
        vfx.SetFloat("Lifetime", DamageVFXLifeTime);
        vfx.SetInt("Digit1", digit1);
        vfx.SetInt("Digit2", digit2);
        vfx.SetVector3("SpawnPosition", spawnPosition);
        vfx.Play();
        StartCoroutine(ReleaseDamageVFX(vfx, DamageVFXLifeTime));
    }

    IEnumerator ReleaseDamageVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        DamageVFXPool.Release(vfx);
        yield break;
    }

    public void PlaySwordSound()
    {
        /*swordAudioSource.clip = SwordSoundEffectPack.GetRandomSoundEffect();
        swordAudioSource.Play();*/
        audioSource.PlayOneShot(SwordSoundEffectPack.GetRandomSoundEffect(), SwordSoundEffectPack.Volume);
        cameraController.ShakeCamera();
    }

    public void PlayWalkSound()
    {
        if (!isComboLayerActive)
            audioSource.PlayOneShot(WalkSoundEffectPack.GetRandomSoundEffect(), WalkSoundEffectPack.Volume);
    }

    public void PlayRunSound()
    {
        if (!isComboLayerActive)
            audioSource.PlayOneShot(RunSoundEffectPack.GetRandomSoundEffect(), RunSoundEffectPack.Volume);
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

    public void AddPower(float amount)
    {
        powerAmount += amount;
        powerAmount = Mathf.Clamp(powerAmount, 0, 100); // 0 ile 100 arasında sınırlandır
        powerBar.SetPower(powerAmount);
        swordMaterial.SetFloat("_EmissionIntensity", (powerAmount / 100f) * SwordMaxEmission);
    }
}
