using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public struct AttackBufferItem
    {
        private float pressedTime;
        public void SetPressedTime(float time)
        {
            pressedTime = time;
        }

        public float GetPressedTime()
        {
            return pressedTime;
        }
    }
    public static PlayerInputManager Instance;
    PlayerInput input;
    PlayerController controller;
    CameraController cameraController;
    public bool inverseCameraY = false;
    public float cameraRotationThreshold = 0.1f;
    public float movementThreshold = 0.1f;
    public bool useMouse = false;
    public float mouseSensitivity = 1f;
    private Vector2 lastMovementDirection = Vector2.zero;
    [HideInInspector]
    public List<AttackBufferItem> attackBuffer = new List<AttackBufferItem>();
    public float attackBufferTime = 0.5f;
    public bool attackStarted = false;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        controller = PlayerController.Instance;
        cameraController = CameraController.Instance;
        input = new PlayerInput();
        input.Player.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>());
        input.Player.Movement.canceled += ctx => Movement(Vector2.zero);
        input.Player.Run.performed += ctx => controller.SetRunState(true);
        input.Player.Run.canceled += ctx => controller.SetRunState(false);
        input.Player.Camera.performed += ctx => Camera(ctx.ReadValue<Vector2>());
        input.Player.Camera.canceled += ctx => Camera(Vector2.zero);
        if (useMouse)
        {
            input.Player.CameraMouse.performed += ctx => Camera(ctx.ReadValue<Vector2>() * mouseSensitivity);
            input.Player.CameraMouse.canceled += ctx => Camera(Vector2.zero);
        }
        input.Player.Attack.performed += ctx => Attack();
        input.Player.Dash.performed += ctx => controller.Dash(lastMovementDirection);
        input.Enable();
    }

    void Update()
    {
        if (attackBuffer.Count > 0)
        {
            if (Time.time - attackBuffer[0].GetPressedTime() > attackBufferTime)
            {
                attackBuffer.RemoveAt(0);
            }
            else if (Time.time - attackBuffer[0].GetPressedTime() < attackBufferTime && !attackStarted)
            {
                attackStarted = true;
                attackBuffer.RemoveAt(0);
                PlayerController.Instance.Attack();
            }
        }
    }

    void Movement(Vector2 direction)
    {
        lastMovementDirection = direction;
        if (direction.magnitude < movementThreshold)
            direction = Vector2.zero;
        controller.SetTargetVelocity(direction.magnitude);
        /*if(direction == Vector2.zero)
            return;*/
        controller.SetMoveDirection(direction);
    }

    void Camera(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < cameraRotationThreshold)
            direction.x = 0;
        if (Mathf.Abs(direction.y) < cameraRotationThreshold)
            direction.y = 0;
        if (inverseCameraY)
            direction.y *= -1;
        //controller.RotateCamera(direction);
        cameraController.RotateCamera(direction);
    }

    void Attack()
    {
        attackBuffer.Add(new AttackBufferItem());
        attackBuffer[attackBuffer.Count - 1].SetPressedTime(Time.time);
        Debug.Log("Attack Buffer Count: " + attackBuffer.Count);
    }
}
