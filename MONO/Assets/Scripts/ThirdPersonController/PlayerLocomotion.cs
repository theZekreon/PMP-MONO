using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager _inputManager;

    Vector3 _moveDirection;
    Transform _cameraObject;
    Rigidbody _playerRigidbody;

    [SerializeField] float _movementSpeed = 7f;
    [SerializeField] float _rotationSpeed = 15f;

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
        _moveDirection = _cameraObject.forward * _inputManager._verticalInput;
        _moveDirection += _cameraObject.right * _inputManager._horizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0; // Prevents player from walking into the air   
        _moveDirection *= _movementSpeed;

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.linearVelocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero; // Zero on all values

        targetDirection = _cameraObject.forward * _inputManager._verticalInput;
        targetDirection += _cameraObject.right * _inputManager._horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) { targetDirection = transform.forward; } // Keep rotation at the position when player stops moving

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime); 

        transform.rotation = playerRotation;
    }
}
