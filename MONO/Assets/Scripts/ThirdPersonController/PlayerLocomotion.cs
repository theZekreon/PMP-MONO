using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager _inputManager;

    Vector3 _moveDirection;
    Transform _cameraObject;
    Rigidbody _playerRigidbody;

    [Header("Movement Speeds")]
    [SerializeField] float _walkingSpeed = 1.5f;
    [SerializeField] float _runningSpeed = 5f;
    [SerializeField] float _sprintingSpeed = 7f;
    [SerializeField] float _rotationSpeed = 15f;

    public bool IsSprinting;

    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
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
}
