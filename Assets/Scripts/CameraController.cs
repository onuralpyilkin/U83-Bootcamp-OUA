using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [Header("Third Person Camera Values")]
    public GameObject ThirdPersonCamera;
    public Transform CameraTarget;
    public float CameraRotationSpeed = 10;
    public float CameraDownLimit = 15, CameraUpLimit = 15;
    private Vector2 cameraRotationInput;

    [Header("Combat Camera Values")]
    public GameObject CombatCamera;
    [HideInInspector]
    public float AngleOnYAxis;
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
        CameraTarget.position = new Vector3(player.transform.position.x, CameraTarget.position.y, player.transform.position.z);
        if (isCombatCameraActive)
            return;
        CameraTarget.rotation *= Quaternion.AngleAxis(cameraRotationInput.x * CameraRotationSpeed * Time.deltaTime, Vector3.up);
        CameraTarget.rotation *= Quaternion.AngleAxis(cameraRotationInput.y * CameraRotationSpeed * Time.deltaTime, Vector3.right);
        Vector3 angles = CameraTarget.localEulerAngles;
        angles.z = 0;
        float clampedAngle = CameraTarget.localEulerAngles.x;
        if (clampedAngle > 180 && clampedAngle < (360 - CameraUpLimit))
        {
            angles.x = 360 - CameraUpLimit;
        }
        else if (clampedAngle < 180 && clampedAngle > (CameraDownLimit))
        {
            angles.x = CameraDownLimit;
        }
        CameraTarget.localEulerAngles = angles;
        AngleOnYAxis = CameraTarget.localEulerAngles.y;
    }

    public void RotateCamera(Vector2 input)
    {
        cameraRotationInput = input;
    }

    public void SetCombatCameraState(bool state)
    {
        isCombatCameraActive = state;
        ThirdPersonCamera.SetActive(!state);
        CombatCamera.SetActive(state);
        if (isCombatCameraActive)
        {
            CameraTarget.localEulerAngles = Vector3.zero;
            AngleOnYAxis = 0;
        }
    }
}
