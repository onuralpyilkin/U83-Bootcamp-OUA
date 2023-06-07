using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Attacks")]
    public string combatIdleAnimName = "Combat Idle";
    public Attack[] attacks;
    public int maxCombo = 3;
    public int comboIndex = 0;
    public float comboExpireTime = 0.5f;
    //private float comboExpireTimer = 0f;
    private float comboStartTime = 0f;
    public float comboValidTime = 0.5f; /*the time between new attack becomes available and last attack finished then waiting for next attack.
                                            so comboValidTime*2 is the available time for the player to press the next attack button.*/
    private Coroutine comboTimer;
    private bool isComboLayerActive = false;
    public float comboLayerWeightChangeSpeed = 5f;
    public float animationSpeed = 1f;
    private int animationSpeedHash;

    [HideInInspector]
    public List<EnemyCapsule> enemies = new List<EnemyCapsule>();
    private EnemyCapsule closestEnemy;
    private Coroutine moveToEnemyCoroutine;
    public float moveToEnemyTime = 1f;

    [Header("Dash")]
    public float dashDistance = 5;
    public float dashCooldown = 1;
    public bool dashAvailable = true;

    [Header("VFX")]
    public VFXPoolController VFXPool;
    public float lifeTime = 1;
    public float particleCount = 100;
    [ColorUsageAttribute(true, true)]
    public Color particleColor = Color.white;

    [Header("Debug")]
    public bool isNextAttackAvailable = false;

    //Other Components
    private CameraController cameraController;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        cameraController = CameraController.Instance;
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        animationSpeedHash = Animator.StringToHash("AnimationSpeed");
        animator.SetFloat(animationSpeedHash, animationSpeed);
        for (int i = 0; i < attacks.Length; i++)
        {
            attacks[i].Initialize();
        }
        int count = VFXPool.GetCount();
        for (int i = 0; i < count; i++)
        {
            VFX vfx = VFXPool.Get();
            vfx.SetFloat("Lifetime", lifeTime);
            vfx.SetFloat("ParticleCount", particleCount);
            vfx.SetVector4("Color", particleColor);
            VFXPool.Release(vfx);
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
        if (currentAnimTime < currentAnimLength - comboValidTime || currentAnimTime > currentAnimLength + comboValidTime)
        {
            isNextAttackAvailable = false;
        }
        else
        {
            isNextAttackAvailable = true;
        }

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
        string currentAnimName = animator.GetCurrentAnimatorClipInfo(1)[0].clip.name;
        float currentAnimLength = animator.GetCurrentAnimatorClipInfo(1).Length;
        float currentAnimTime = animator.GetCurrentAnimatorStateInfo(1).normalizedTime * currentAnimLength;
        if (currentAnimName != combatIdleAnimName && currentAnimTime < currentAnimLength - comboValidTime)
        {
            return;
        }

        if (comboIndex + 1 >= maxCombo || Time.time - comboStartTime >= comboExpireTime)
        {
            comboIndex = 0;
        }

        comboStartTime = Time.time;
        if (comboIndex == 0)
        {
            attacks = ShuffleArray(attacks);
            comboTimer = StartCoroutine(ComboTimer());
        }
        animator.SetTrigger(attacks[comboIndex].triggerHash);
        comboIndex++;
        if (moveToEnemyCoroutine != null)
        {
            StopCoroutine(moveToEnemyCoroutine);
        }
        closestEnemy = GetClosestEnemy();
        if (closestEnemy != null)
            moveToEnemyCoroutine = StartCoroutine(MoveToEnemy(closestEnemy));
    }

    IEnumerator ComboTimer()
    {
        isComboLayerActive = true;
        while (Time.time - comboStartTime < comboExpireTime)
        {
            //Debug.Log("Combo Timer: " + (Time.time - comboStartTime));
            yield return null;
        }
        comboIndex = 0;
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
        if (!dashAvailable || VFXPool.GetCount() == 0)
            return;
        StartCoroutine(DashCooldown());
        Vector3 dashDirection = transform.forward;
        dashDirection.y = 0;
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
        VFX vfx = VFXPool.Get();
        vfx.SetPosition(transform.position);
        vfx.SetRotation(transform.rotation);
        vfx.Play();
        StartCoroutine(ReleaseVFX(vfx, lifeTime));
    }

    IEnumerator ReleaseVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        VFXPool.Release(vfx);
        yield break;
    }
}
