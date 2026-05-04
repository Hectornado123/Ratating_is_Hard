using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] InputActionReference lookAction;

    [Header("Rotation Settings")]
    [SerializeField] float maxSpeed = 220f;
    [SerializeField] float acceleration = 6f;
    [SerializeField] float deceleration = 4f;
    [SerializeField] float smoothTime = 0.05f;

    [Header("Vertical Clamp")]
    [SerializeField] float minY = -30f;
    [SerializeField] float maxY = 60f;

    float yaw;
    float pitch;

    float yawVelocitySmooth;
    float pitchVelocitySmooth;

    float currentSpeedX;
    float currentSpeedY;

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        Vector2 input = lookAction.action.ReadValue<Vector2>();

        float inputX = Mathf.Clamp(input.x, -1f, 1f);
        float inputY = Mathf.Clamp(input.y, -1f, 1f);

        //  ACELERACIÓN / FRENADO
        float targetSpeedX = inputX * maxSpeed;
        float targetSpeedY = inputY * maxSpeed;

        float accelX = Mathf.Abs(inputX) > 0.01f ? acceleration : deceleration;
        float accelY = Mathf.Abs(inputY) > 0.01f ? acceleration : deceleration;

        currentSpeedX = Mathf.Lerp(currentSpeedX, targetSpeedX, accelX * Time.deltaTime);
        currentSpeedY = Mathf.Lerp(currentSpeedY, targetSpeedY, accelY * Time.deltaTime);

        //  Rotación
        yaw += currentSpeedX * Time.deltaTime;
        pitch -= currentSpeedY * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minY, maxY);

        //  Suavizado final
        float smoothYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, yaw, ref yawVelocitySmooth, smoothTime);
        float smoothPitch = Mathf.SmoothDampAngle(transform.eulerAngles.x, pitch, ref pitchVelocitySmooth, smoothTime);

        transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
    }
}