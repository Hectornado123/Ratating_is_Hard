using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;

    [Header("Referencia a la cámara")]
    public Transform cameraTransform;

    [Header("Animación")]
    public Animator animator;

    [Header("Rotación")]
    public float rotationSpeed = 10f;

    [Tooltip("Offset de rotación del modelo")]
    public Vector3 modelRotationOffset;

    [Header("Modelo visual")]
    public Transform visualModel;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Evita inclinaciones raras
        rb.freezeRotation = true;
    }

    // INPUT SYSTEM
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (cameraTransform == null)
            return;

        // Dirección relativa a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Dirección de movimiento
        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        moveDirection.Normalize();

        // Movimiento
        Vector3 velocity = moveDirection * speed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        // Rotación
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            // Rotación física del player
            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );

            // Rotación visual + offset
            if (visualModel != null)
            {
                Quaternion visualRotation =
                    targetRotation *
                    Quaternion.Euler(modelRotationOffset);

                visualModel.rotation = Quaternion.Slerp(
                    visualModel.rotation,
                    visualRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );
            }
        }

        // Animaciones
        if (animator != null)
        {
            animator.SetBool(
                "isWalking",
                moveDirection.sqrMagnitude > 0.001f
            );
        }
    }
}