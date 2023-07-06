using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputManager : MonoBehaviour
{
    public enum AttackType
    {
        Attack,
        Dash
    }
    public class AttackBufferItem
    {
        private float pressedTime;
        private AttackType attackType;
        public void SetPressedTime(float time)
        {
            pressedTime = time;
        }

        public float GetPressedTime()
        {
            return pressedTime;
        }

        public void SetAttackType(AttackType type)
        {
            attackType = type;
        }

        public AttackType GetAttackType()
        {
            return attackType;
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
    //private Vector2 lastMovementDirection = Vector2.zero;
    [HideInInspector]
    public List<AttackBufferItem> AttackBuffer = new List<AttackBufferItem>();
    public float AttackBufferTime = 0.5f;
    public bool AttackStarted = false;
    [HideInInspector]
    public UnityEvent OnSubmit;
    [HideInInspector]
    public bool LatestInputIsGamepad = false;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    void Start()
    {
        controller = PlayerController.Instance;
        cameraController = CameraController.Instance;
        input = new PlayerInput();
        input.Player.Movement.performed += ctx =>
        {
            Movement(ctx.ReadValue<Vector2>());
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Movement.canceled += ctx =>
        {
            Movement(Vector2.zero);
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Run.performed += ctx =>
        {
            controller.SetRunState(true);
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Run.canceled += ctx =>
        {
            controller.SetRunState(false);
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Camera.performed += ctx =>
        {
            Camera(ctx.ReadValue<Vector2>());
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Camera.canceled += ctx =>
        {
            Camera(Vector2.zero);
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        if (UseMouse)
        {
            input.Player.CameraMouse.performed += ctx =>
            {
                Camera(ctx.ReadValue<Vector2>() * MouseSensitivity);
                SetLatestDeviceType(false);
            };
            input.Player.CameraMouse.canceled += ctx =>
            {
                Camera(Vector2.zero);
                SetLatestDeviceType(false);
            };
        }
        input.Player.Attack.performed += ctx =>
        {
            Attack();
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Dash.performed += ctx =>
        {
            Attack(AttackType.Dash);
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Player.Dodge.performed += ctx =>
        {
            controller.Dodge();
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.UI.Submit.performed += ctx =>
        {
            OnSubmit.Invoke();
            SetLatestDeviceType(ctx.control.device is Gamepad);
        };
        input.Enable();
        input.Player.Enable();
        input.UI.Disable();
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

                if (AttackBuffer[0].GetAttackType() == AttackType.Dash)
                    PlayerController.Instance.Dash(input.Player.Movement.ReadValue<Vector2>());
                else if (AttackBuffer[0].GetAttackType() == AttackType.Attack)
                    PlayerController.Instance.Attack();

                AttackBuffer.RemoveAt(0);
            }
        }
    }

    void SetLatestDeviceType(bool isGamepad)
    {
        LatestInputIsGamepad = isGamepad;
    }

    void Movement(Vector2 direction)
    {
        //lastMovementDirection = direction;
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

    void Attack(AttackType type = AttackType.Attack)
    {
        AttackBuffer.Add(new AttackBufferItem());
        AttackBuffer[AttackBuffer.Count - 1].SetPressedTime(Time.time);
        AttackBuffer[AttackBuffer.Count - 1].SetAttackType(type);
    }

    public void EnableInput()
    {
        input.Enable();
    }

    public void DisableInput()
    {
        input.Disable();
    }

    public void EnablePlayerInput()
    {
        input.Player.Enable();
    }

    public void DisablePlayerInput()
    {
        input.Player.Disable();
    }

    public void EnableUIInput()
    {
        input.UI.Enable();
    }

    public void DisableUIInput()
    {
        input.UI.Disable();
    }
}
