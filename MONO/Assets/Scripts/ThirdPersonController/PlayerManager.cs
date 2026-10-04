using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager _inputManager;
    CameraManager _cameraManager;
    PlayerLocomotion _playerLocomotion;

    void Awake() // Could also use [SerializeField] for the variables above 
    {
        _inputManager = GetComponent<InputManager>();
        _cameraManager = Object.FindFirstObjectByType<CameraManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        _inputManager.HandleAllInputs();
    }

    void FixedUpdate()
    {
        _playerLocomotion.HandleAllMovement();
    }

    void LateUpdate() // Works almost the same as Update(), but calls after the frame has ended
    {
        _cameraManager.HandleAllCameraMovement();
    }
}
