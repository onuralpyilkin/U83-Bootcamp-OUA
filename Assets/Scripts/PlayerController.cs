using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Movement Animation Thresholds")]
    public float walkThreshold = 0.1f;
    public float runThreshold = 0.9f;
    public float combatWalkThreshold = 0.1f;

    [Header("Movement Values")]
    public float acceleration = 1f;
    public float deceleration = 1f;
    public float turnSpeed = 1f;

    private float velocity, targetVelocity;
    private bool isRunning;
    public float angle, targetAngle; //angles on the Y axis
    private Vector2 moveDirection;

    //Animator variables
    private Animator animator;
    private int velocityHash;

    [Header("Combos")]
    public Combo[] combos;
    private int currentComboIndex = 0;
    private int currentAttackIndex = 0;
    public float comboExpireTime = 0.5f;
    [HideInInspector]
    public Coroutine comboExpireTimer;
    private bool isComboLayerActive = false;
    public float comboLayerWeightChangeSpeed = 5f;
    public float animationSpeed = 1f;
    private int animationSpeedHash;
    [HideInInspector]
    public bool isAttacking = false;
    [HideInInspector]
    public float comboExpireStartTime = 0f;
    private bool isComboActive = false;

    [HideInInspector]
    public List<EnemyCapsule> enemies = new List<EnemyCapsule>();
    private EnemyCapsule closestEnemy;
    private Coroutine moveToEnemyCoroutine;
    public float moveToEnemyTime = 1f;

    [Header("Dash")]
    public float dashDistance = 5;
    public float dashCooldown = 1;
    public bool dashAvailable = true;

    [Header("Dash VFX")]
    public VFXPoolController dashVFXPool;
    public float dashVFXLifeTime = 1;
    public float dashVFXParticleCount = 100;
    [ColorUsageAttribute(true, true)]
    public Color dashVFXParticleColor = Color.white;

    //Other Components
    private CameraController cameraController;
    private PlayerInputManager inputManager;

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
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        animationSpeedHash = Animator.StringToHash("AnimationSpeed");
        animator.SetFloat(animationSpeedHash, animationSpeed);
        for (int i = 0; i < combos.Length; i++)
        {
            combos[i].Initialize();
        }
        int count = dashVFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = dashVFXPool.Get();
            vfx.SetFloat("Lifetime", dashVFXLifeTime);
            vfx.SetFloat("ParticleCount", dashVFXParticleCount);
            vfx.SetVector4("Color", dashVFXParticleColor);
            dashVFXPool.Release(vfx);
        }
    }

    void Update()
    {
        //Set Velocity
        velocity = Mathf.MoveTowards(velocity, targetVelocity * (isComboLayerActive ? combatWalkThreshold : (isRunning ? runThreshold : walkThreshold)), (velocity <= targetVelocity ? acceleration : deceleration) * Time.deltaTime);
        animator.SetFloat(velocityHash, velocity);

        //Set Y Angle
        if (velocity != 0 && moveDirection != Vector2.zero)
            targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cameraController.angleOnYAxis;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Debug
        float currentAnimLength = animator.GetCurrentAnimatorClipInfo(1).Length;
        float currentAnimTime = animator.GetCurrentAnimatorStateInfo(1).normalizedTime * currentAnimLength;

        animator.SetLayerWeight(1, Mathf.MoveTowards(animator.GetLayerWeight(1), isComboLayerActive ? 1 : 0, Time.deltaTime * comboLayerWeightChangeSpeed));
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
        if (currentAttackIndex + 1 > combos[currentComboIndex].GetComboLength() || !isComboActive)
        {
            currentAttackIndex = 0;
        }

        if (currentAttackIndex == 0)
        {
            currentComboIndex = Random.Range(0, combos.Length);
            //comboTimer = StartCoroutine(ComboExpireTimer());
        }

        isComboLayerActive = true;
        isComboActive = true;
        isAttacking = true;
        animator.SetTrigger(combos[currentComboIndex].attacks[currentAttackIndex].triggerHash);
        currentAttackIndex++;
        if (moveToEnemyCoroutine != null)
        {
            StopCoroutine(moveToEnemyCoroutine);
        }
        closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
            moveToEnemyCoroutine = StartCoroutine(MoveToEnemy(closestEnemy));
    }

    public IEnumerator ComboExpireTimer()
    {
        comboExpireStartTime = Time.time;
        while (Time.time - comboExpireStartTime < comboExpireTime)
        {
            yield return null;
        }
        isComboActive = false;
        currentAttackIndex = 0;
        isComboLayerActive = false;
    }

    IEnumerator MoveToEnemy(EnemyCapsule enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        float speed = distance / moveToEnemyTime;
        while (distance > enemy.playerFightDistance)
        {
            Vector3 targetPos = enemy.transform.position;
            targetPos.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, targetPos);
            yield return new WaitForEndOfFrame();
        }
    }

    EnemyCapsule GetClosestEnemy()
    {
        //get closest enemy by moveDirection value (dot product)
        if (this.closestEnemy != null && moveDirection == Vector2.zero)
        {
            return this.closestEnemy;
        }
        float closestEnemyDot = -1f;
        EnemyCapsule closestEnemy = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            float dot = Vector3.Dot(transform.forward, (enemies[i].transform.position - transform.position).normalized);
            if (dot > closestEnemyDot && enemies[i].isInRange)
            {
                closestEnemyDot = dot;
                closestEnemy = enemies[i];
            }
        }
        return closestEnemy;
    }

    public void Dash(Vector2 dir)
    {
        if (!dashAvailable || dashVFXPool.GetCount() == 0)
            return;
        StartCoroutine(DashCooldown());
        Vector3 dashDirection = new Vector3(dir.x, 0, dir.y);
        PlayVFX();
        transform.position += dashDirection * dashDistance;
        PlayVFX();
    }

    IEnumerator DashCooldown()
    {
        dashAvailable = false;
        yield return new WaitForSeconds(dashCooldown);
        dashAvailable = true;
        yield break;
    }

    void PlayVFX()
    {
        VFX vfx = dashVFXPool.Get();
        vfx.SetPosition(transform.position);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        StartCoroutine(ReleaseVFX(vfx, dashVFXLifeTime));
    }

    IEnumerator ReleaseVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        dashVFXPool.Release(vfx);
        yield break;
    }
}
