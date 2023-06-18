using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public class AttackBufferItem
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
    public bool InverseCameraY = false;
    public float CameraRotationThreshold = 0.1f;
    public float MovementThreshold = 0.1f;
    public bool UseMouse = false;
    public float MouseSensitivity = 1f;
    private Vector2 lastMovementDirection = Vector2.zero;
    [HideInInspector]
    public List<AttackBufferItem> AttackBuffer = new List<AttackBufferItem>();
    public float AttackBufferTime = 0.5f;
    public bool AttackStarted = false;

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
        if (UseMouse)
        {
            input.Player.CameraMouse.performed += ctx => Camera(ctx.ReadValue<Vector2>() * MouseSensitivity);
            input.Player.CameraMouse.canceled += ctx => Camera(Vector2.zero);
        }
        input.Player.Attack.performed += ctx => Attack();
        input.Player.Dash.performed += ctx => controller.Dash(lastMovementDirection);
        input.Enable();
    }

    void Update()
    {
        if (AttackBuffer.Count > 0)
        {
            if (Time.time - AttackBuffer[0].GetPressedTime() > AttackBufferTime)
            {
                AttackBuffer.RemoveAt(0);
            }
            else if (Time.time - AttackBuffer[0].GetPressedTime() < AttackBufferTime && !AttackStarted)
            {
                AttackStarted = true;
                AttackBuffer.RemoveAt(0);
                PlayerController.Instance.Attack();
            }
        }
    }

    void Movement(Vector2 direction)
    {
        lastMovementDirection = direction;
        if (direction.magnitude < MovementThreshold)
            direction = Vector2.zero;
        controller.SetTargetVelocity(direction.magnitude);
        /*if(direction == Vector2.zero)
            return;*/
        controller.SetMoveDirection(direction);
    }

    void Camera(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < CameraRotationThreshold)
            direction.x = 0;
        if (Mathf.Abs(direction.y) < CameraRotationThreshold)
            direction.y = 0;
        if (InverseCameraY)
            direction.y *= -1;
        //controller.RotateCamera(direction);
        cameraController.RotateCamera(direction);
    }

    void Attack()
    {
        AttackBuffer.Add(new AttackBufferItem());
        AttackBuffer[AttackBuffer.Count - 1].SetPressedTime(Time.time);
    }
}
