using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionAsset InputActions;

    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float runSpeed = 10f;
    [SerializeField] private float rotationSmoothTime = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private bool runInput;
    private float turnSmoothVelocity;
    private bool isGrounded;



    private void Awake()
    {
        moveAction = InputActions.FindAction("Player/Move");
        jumpAction = InputActions.FindAction("Player/Jump");

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // daha dengeli z�plama
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        jumpAction.performed -= OnJump;
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        isGrounded = CheckGrounded();

        // Animator g�ncelleme
        if (animator)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
            animator.SetBool("IsGrounded", isGrounded);
        }

    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);

        rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        if (runInput)
            rb.MovePosition(rb.position + moveDir.normalized * runSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (animator)
        {
            //animator.SetTrigger("IsJumped");
        }
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundLayer);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 direction = Vector3.down;

        // Raycast ile gerçek çarpma noktasını al ve ona göre renk/sphere çiz
        if (Physics.Raycast(origin, direction, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.05f);
            Gizmos.DrawWireSphere(origin, 0.02f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + direction * groundCheckDistance);
            Gizmos.DrawWireSphere(origin, 0.02f);
        }
    }
#endif

}