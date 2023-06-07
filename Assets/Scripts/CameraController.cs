using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [Header("Third Person Camera Values")]
    public GameObject thirdPersonCamera;
    public Transform cameraTarget;
    public float cameraRotationSpeed = 10;
    public float cameraDownLimit = 15, cameraUpLimit = 15;
    private Vector2 cameraRotationInput;

    [Header("Combat Camera Values")]
    public GameObject combatCamera;
    [HideInInspector]
    public float angleOnYAxis;
    PlayerController player;
    private bool isCombatCameraActive = false;

    void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }
    void Start()
    {
        player = PlayerController.Instance;
    }

    void Update()
    {
        cameraTarget.position = new Vector3(player.transform.position.x, cameraTarget.position.y, player.transform.position.z);
        if (isCombatCameraActive)
            return;
        cameraTarget.rotation *= Quaternion.AngleAxis(cameraRotationInput.x * cameraRotationSpeed * Time.deltaTime, Vector3.up);
        cameraTarget.rotation *= Quaternion.AngleAxis(cameraRotationInput.y * cameraRotationSpeed * Time.deltaTime, Vector3.right);
        Vector3 angles = cameraTarget.localEulerAngles;
        angles.z = 0;
        float clampedAngle = cameraTarget.localEulerAngles.x;
        if (clampedAngle > 180 && clampedAngle < (360 - cameraUpLimit))
        {
            angles.x = 360 - cameraUpLimit;
        }
        else if (clampedAngle < 180 && clampedAngle > (cameraDownLimit))
        {
            angles.x = cameraDownLimit;
        }
        cameraTarget.localEulerAngles = angles;
        angleOnYAxis = cameraTarget.localEulerAngles.y;
    }

    public void RotateCamera(Vector2 input)
    {
        cameraRotationInput = input;
    }

    public void SetCombatCameraState(bool state)
    {
        isCombatCameraActive = state;
        thirdPersonCamera.SetActive(!state);
        combatCamera.SetActive(state);
        if (isCombatCameraActive)
        {
            cameraTarget.localEulerAngles = Vector3.zero;
            angleOnYAxis = 0;
        }
    }
}
