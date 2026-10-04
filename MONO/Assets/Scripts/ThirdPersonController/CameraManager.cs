using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager _inputManager;

    [SerializeField] Transform _targetTransform; // The object the camera will follow
    [SerializeField] Transform _cameraPivot; // The object the camera uses to pivot (look up and down)
    [SerializeField] Transform _cameraTransform; // The transform of the actual camera object in the scene
    [SerializeField] LayerMask _collisionLayers; // The layers we want our layers to collide with
    float _defaultPosition;
    Vector3 _cameraFollowVelocity = Vector3.zero;
    Vector3 _cameraVectorPosition;

    [SerializeField] float _cameraFollowSpeed = 0.2f;
    [SerializeField] float _cameraLookSpeed = 2f;
    [SerializeField] float _cameraPivotSpeed = 2f;
    [SerializeField] float _cameraCollisionRadius = 0.2f;
    [SerializeField] float _cameraCollisionOffset = 0.2f; // How much the camera will jump off of objects its colliding with
    [SerializeField] float _minimumCollisionOffset = 0.2f;

    [SerializeField] float _lookAngle; // Camera looking up and down
    [SerializeField] float _pivotAngle; // Camera looking left and right
    [SerializeField] float _minimumPivotAngle = -35f;
    [SerializeField] float _maximumPivotAngle = 35f;

    void Awake() // Could also use [SerializeField] for the variables above (that are inside of the Awake() method)
    {
        _inputManager = Object.FindFirstObjectByType<InputManager>();
        _targetTransform = Object.FindFirstObjectByType<PlayerManager>().transform;
        _cameraTransform = Camera.main.transform;
        _defaultPosition = _cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    void FollowTarget() // Target = Player
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position, ref _cameraFollowVelocity, _cameraFollowSpeed);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        _lookAngle += (_inputManager._cameraInputX * _cameraLookSpeed);
        _pivotAngle -= (_inputManager._cameraInputY * _cameraPivotSpeed);
        _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivotAngle, _maximumPivotAngle);

        Vector3 rotation;
        Quaternion targetRotation;

        rotation = Vector3.zero;
        rotation.y = _lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        _cameraPivot.localRotation = targetRotation;
    }

    void HandleCameraCollisions()
    {
        float targetPosition = _defaultPosition;
        RaycastHit hit;
        Vector3 direction = _cameraTransform.position - _cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(_cameraPivot.transform.position, _cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), _collisionLayers))
        {
            float distance = Vector3.Distance(_cameraPivot.position, hit.point);
            targetPosition =- (distance - _cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < _minimumCollisionOffset)
        {
            targetPosition =- _minimumCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
        _cameraTransform.localPosition = _cameraVectorPosition;
    }
}
