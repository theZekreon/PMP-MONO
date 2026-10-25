using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Calls the PlayerControls script that was made with Unity's input system
    PlayerControls _playerControls;
    PlayerLocomotion _playerLocomotion;
    AnimatorManager _animatorManager;

    public Vector2 MovementInput;
    public Vector3 CameraInput;

    public float CameraInputX;
    public float CameraInputY;

    public float MoveAmount;
    public float HorizontalInput;
    public float VerticalInput;

    public bool SprintInput;

    void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void OnEnable()
    {
        if(_playerControls == null)
        {
            _playerControls = new PlayerControls();

            _playerControls.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();
            _playerControls.PlayerMovement.Camera.performed += i => CameraInput = i.ReadValue<Vector2>();

            _playerControls.PlayerActions.Sprint.performed += i => SprintInput = true;
            _playerControls.PlayerActions.Sprint.canceled += i => SprintInput = false;
        }

        _playerControls.Enable();
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
    }

    void HandleMovementInput()
    {
        HorizontalInput = MovementInput.x;
        VerticalInput = MovementInput.y;

        CameraInputX = CameraInput.x;
        CameraInputY = CameraInput.y;

        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput)); // Abs takes away sign in front of the value
        _animatorManager.UpdateAnimatorValues(0, MoveAmount, _playerLocomotion.IsSprinting);
    }

    void HandleSprintingInput()
    {
        if (SprintInput && MoveAmount > 0.5f)
        {
            _playerLocomotion.IsSprinting = true;
        }
        else
        {
            _playerLocomotion.IsSprinting = false;
        }
    }
}