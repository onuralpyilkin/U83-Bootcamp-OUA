using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    PlayerInput input;
    PlayerController controller;
    public bool inverseCameraY = false;
    public float cameraRotationThreshold = 0.1f;
    public float movementThreshold = 0.1f;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        input = new PlayerInput();
        input.Player.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>());
        input.Player.Movement.canceled += ctx => Movement(Vector2.zero);
        input.Player.Run.performed += ctx => controller.SetRunState(true);
        input.Player.Run.canceled += ctx => controller.SetRunState(false);
        input.Player.Camera.performed += ctx => Camera(ctx.ReadValue<Vector2>());
        input.Player.Camera.canceled += ctx => Camera(Vector2.zero);
        input.Enable();
    }

    void Movement(Vector2 direction)
    {
        if(direction.magnitude < movementThreshold)
            direction = Vector2.zero;
        controller.SetTargetVelocity(direction.magnitude);
        if(direction == Vector2.zero)
            return;
        controller.SetMoveDirection(direction);
    }

    void Camera(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) < cameraRotationThreshold)
            direction.x = 0;
        if(Mathf.Abs(direction.y) < cameraRotationThreshold)
            direction.y = 0;
        if(inverseCameraY)
            direction.y *= -1;
        controller.RotateCamera(direction);
    }
}
