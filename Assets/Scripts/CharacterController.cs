using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ESTE es el que debe aparecer sí o sí
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        rb.linearVelocity = new Vector3(
            move.x * speed,
            rb.linearVelocity.y,
            move.z * speed
        );
    }
}