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

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Evita inclinaciones raras
        rb.freezeRotation = true;
    }

    // Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (cameraTransform == null)
            return;

        // Dirección de cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Dirección movimiento
        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        // Normalizar para evitar más velocidad en diagonal
        moveDirection.Normalize();

        // Movimiento
        Vector3 velocity = moveDirection * speed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        // Rotación suave SOLO si se mueve
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
        }

        // Animación
        if (animator != null)
        {
            animator.SetBool(
                "isWalking",
                moveDirection.sqrMagnitude > 0.001f
            );
        }
    }
}