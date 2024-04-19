using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 4.0f;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    // Movement Input
    private Vector2 moveDirection;
    private bool isRunning = false;
    private float horizontal;
    private float vertical;
    private float inputLerpingSpeed = 10.0f;
    private float movementSpeed;
    // Animator
    private int animHorizontalHash = Animator.StringToHash("Horizontal");
    private int animVerticalHash = Animator.StringToHash("Vertical");
    private int animSpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0.0f;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        HandleAnimatorParameters();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (isRunning)
        {
            rb.velocity = moveDirection * movementSpeed * runSpeed;
        }
        else
        {
            rb.velocity = moveDirection * movementSpeed * walkSpeed;
        }
    }

    private void ProcessInput()
    {
        float horizontalInput = InputManager.Instance.CharacterKeybinds.GetHorizontalAxisInput();
        float verticalInput = InputManager.Instance.CharacterKeybinds.GetVerticalAxisInput();

        horizontal = Mathf.Lerp(horizontal, horizontalInput, inputLerpingSpeed * Time.deltaTime);
        vertical = Mathf.Lerp(vertical, verticalInput, inputLerpingSpeed * Time.deltaTime);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            moveDirection = Vector2.zero;
        }
        else
        {
            moveDirection = new Vector2(horizontal, vertical).normalized;
        }

        movementSpeed = moveDirection.magnitude;

        isRunning = Input.GetKey(InputManager.Instance.CharacterKeybinds.RunKey);
    }

    private void HandleAnimatorParameters()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat(animHorizontalHash, moveDirection.x);
            animator.SetFloat(animVerticalHash, moveDirection.y);
        }
        animator.SetFloat(animSpeedHash, movementSpeed);
    }
}
