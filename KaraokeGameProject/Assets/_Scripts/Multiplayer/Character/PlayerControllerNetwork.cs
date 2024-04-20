using Unity.Netcode;
using UnityEngine;

public class PlayerControllerNetwork : NetworkBehaviour
{
    [Header("Movement Configurations")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 4.0f;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    // Movement
    private Vector2 moveDirection;
    private float moveSpeed;
    private bool isRunning;
    // Animations
    private int animHorizontalId = Animator.StringToHash("Horizontal");
    private int animVerticalId = Animator.StringToHash("Vertical");
    private int animSpeedId = Animator.StringToHash("Speed");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        HandleInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        HandleMovement();
    }

    private void HandleInput()
    {
        moveDirection = InputManager.Instance.MoveInput;
        moveSpeed = moveDirection.magnitude;
        isRunning = InputManager.Instance.RunBeingHeld;
    }

    private void HandleMovement()
    {
        if (!isRunning)
        {
            rb.velocity = moveDirection * walkSpeed;
        }
        else
        {
            rb.velocity = moveDirection * runSpeed;
        }
    }

    private void HandleAnimation()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat(animHorizontalId, moveDirection.x);
            animator.SetFloat(animVerticalId, moveDirection.y);
        }
        float modifiedMoveSpeed = isRunning ? moveSpeed * 2 : moveSpeed;
        animator.SetFloat(animSpeedId, modifiedMoveSpeed);
    }
}
