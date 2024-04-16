using Unity.Netcode;
using UnityEngine;

public class CharacterMovementNetwork : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 4.0f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private Vector2 moveDirection;
    private bool isRunning = false;
    private float horizontal;
    private float vertical;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        ProcessInput();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (isRunning)
        {
            rb.velocity = moveDirection * runSpeed;
        }
        else
        {
            rb.velocity = moveDirection * walkSpeed;
        }
    }

    private void ProcessInput()
    {
        // Horizontal
        if (Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveLeft) || Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveRight))
        {
            horizontal = Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveLeft) ? -1.0f : 1.0f;
        }
        else
        {
            horizontal = 0.0f;
        }
        // Vertical
        if (Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveUp) || Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveDown))
        {
            vertical = Input.GetKey(InputManager.Instance.CharacterKeybinds.MoveDown) ? -1.0f : 1.0f;
        }
        else
        {
            vertical = 0.0f;
        }
        moveDirection = new Vector2(horizontal, vertical).normalized;
        isRunning = Input.GetKey(InputManager.Instance.CharacterKeybinds.RunKey);
    }
}
