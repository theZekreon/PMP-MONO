using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager _playerManager;
    InputManager _inputManager;
    AnimatorManager _animatorManager;

    Vector3 _moveDirection;
    Transform _cameraObject;
    Rigidbody _playerRigidbody;

    [Header("Movement Flags")]
    public bool IsSprinting;
    public bool IsGrounded;

    [Header("Falling")]
    [SerializeField] float _inAirTimer;
    [SerializeField] float _leapingVelocity;
    [SerializeField] float _fallingVelocity;
    [SerializeField] float _raycastHeightOffset = 0.5f;
    [SerializeField] LayerMask _groundLayer;

    [Header("Movement Speeds")]
    [SerializeField] float _walkingSpeed = 1.5f;
    [SerializeField] float _runningSpeed = 5f;
    [SerializeField] float _sprintingSpeed = 7f;
    [SerializeField] float _rotationSpeed = 15f;

    void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _inputManager = GetComponent<InputManager>();
        _animatorManager = GetComponent<AnimatorManager>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        
        if (_playerManager.IsInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        _moveDirection = _cameraObject.forward * _inputManager.VerticalInput;
        _moveDirection += _cameraObject.right * _inputManager.HorizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0; // Prevents player from walking into the air   

        // If the player is walking, select the walking speed
        // If the player is running, select the running speed
        // If the player is sprinting, select the sprinting speed
        if (IsSprinting)
        {
            _moveDirection *= _sprintingSpeed;
        }
        else
        {
            if (_inputManager.MoveAmount >= 0.5f)
            {
                _moveDirection *= _runningSpeed;
            }
            else
            {
                _moveDirection *= _walkingSpeed;
            }
        }

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.linearVelocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero; // Zero on all values

        targetDirection = _cameraObject.forward * _inputManager.VerticalInput;
        targetDirection += _cameraObject.right * _inputManager.HorizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) { targetDirection = transform.forward; } // Keep rotation at the position when player stops moving

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime); 

        transform.rotation = playerRotation;
    }

    void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += _raycastHeightOffset;

        if (!IsGrounded)
        {
            if (!_playerManager.IsInteracting)
            {
                _animatorManager.PlayTargetAnimation("Falling", true);
            }

            _inAirTimer += Time.deltaTime;
            _playerRigidbody.AddForce(transform.forward * _leapingVelocity);
            _playerRigidbody.AddForce(-Vector3.up * _fallingVelocity * _inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, _groundLayer))
        {
            if (!IsGrounded && _playerManager.IsInteracting)
            {
                _animatorManager.PlayTargetAnimation("Landing", true);
            }

            _inAirTimer = 0;
            IsGrounded = true;
        }
        else 
        { 
            IsGrounded = false;
        }
    }
}