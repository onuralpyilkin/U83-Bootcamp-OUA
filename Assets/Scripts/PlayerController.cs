using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Animation Thresholds")]
    public float walkThreshold = 0.1f;
    public float runThreshold = 0.9f;

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

    [Header("Camera Values")]
    public Transform cameraTargetParent;
    public Transform cameraTarget;
    public float cameraRotationSpeed = 10;
    public float cameraDownLimit = 15, cameraUpLimit = 15;
    private Vector2 cameraRotationInput;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        //Set Velocity
        velocity = Mathf.MoveTowards(velocity, targetVelocity * (isRunning ? runThreshold : walkThreshold), (velocity <= targetVelocity ? acceleration : deceleration) * Time.deltaTime);
        animator.SetFloat(velocityHash, velocity);

        //Set Y Angle
        if(velocity != 0)
            targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + cameraTargetParent.eulerAngles.y;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Set Camera Target Position and Rotation
        cameraTargetParent.position = new Vector3(transform.position.x, cameraTarget.position.y, transform.position.z);
        cameraTargetParent.rotation *= Quaternion.AngleAxis(cameraRotationInput.x * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        cameraTarget.rotation *= Quaternion.AngleAxis(cameraRotationInput.y * cameraRotationSpeed * Time.deltaTime, Vector3.right);
        float clampedAngle = cameraTarget.localEulerAngles.x;
        if (clampedAngle > 180 && clampedAngle < (360 - cameraUpLimit))
        {
            clampedAngle = 360 - cameraUpLimit;
        }
        else if (clampedAngle < 180 && clampedAngle > (cameraDownLimit))
        {
            clampedAngle = cameraDownLimit;
        }
        cameraTarget.localEulerAngles = new Vector3(clampedAngle, 0, 0);
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

    public void RotateCamera(Vector2 rotation)
    {
        cameraRotationInput = rotation;
    }
}
