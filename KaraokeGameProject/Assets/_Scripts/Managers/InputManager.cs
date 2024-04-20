using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    // Raw Inputs
    public Vector2 MoveInput { get; private set; }
    public bool RunJustPressed { get; private set; }
    public bool RunBeingHeld { get; private set; }
    public bool RunReleased { get; private set; }
    public bool InteractInput { get; private set; }
    // Player Input
    private PlayerInput playerInput;
    // Input Actions
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactAction;

    public override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        SetInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetInputActions()
    {
        moveAction = playerInput.actions["Movement"];
        runAction = playerInput.actions["Run"];
        interactAction = playerInput.actions["Interact"];
    }

    private void UpdateInputs()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        RunJustPressed = runAction.WasPressedThisFrame();
        RunBeingHeld = runAction.IsPressed();
        RunReleased = runAction.WasReleasedThisFrame();
        InteractInput = interactAction.WasPressedThisFrame();
    }
}
