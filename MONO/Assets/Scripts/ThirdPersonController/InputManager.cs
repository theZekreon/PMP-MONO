using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Calls the PlayerControls script that was made with Unity's input system
    PlayerControls _playerControls;
    AnimatorManager _animatorManager;

    public Vector2 _movementInput;
    public Vector3 _cameraInput;

    public float _cameraInputX;
    public float _cameraInputY;

    float _moveAmount;
    public float _horizontalInput;
    public float _verticalInput;

    void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
    }

    void OnEnable()
    {
        if(_playerControls == null)
        {
            _playerControls = new PlayerControls();

            _playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
            _playerControls.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
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
    }

    void HandleMovementInput()
    {
        _horizontalInput = _movementInput.x;
        _verticalInput = _movementInput.y;

        _cameraInputX = _cameraInput.x;
        _cameraInputY = _cameraInput.y;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput)); // Abs takes away sign in front of the value
        _animatorManager.UpdateAnimatorValues(0, _moveAmount);
    }
}
