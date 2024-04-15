using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 4.0f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 moveDirection;
    private bool isRunning = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
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
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(xAxis, yAxis).normalized;
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }
}
