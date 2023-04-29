using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraContDrager : MonoBehaviour
{
    public static CameraContDrager Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;


    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _dragInput;
    private float _zoomInput;
    private Vector2 _mousePosInput;


    private Vector3 cameraMove = Vector3.zero;
    private Camera _mainCamera;


    public Transform CameraFollowTarget;

    // Initializing Singleton and InputManager
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SubscribeToInput();
    }

    private void SubscribeToInput()
    {
        InputManager.OnMove += OnMoveInput;
        InputManager.OnLook += OnLookInput;
        InputManager.OnDrag += OnDragInput;
        InputManager.OnZoom += OnZoomInput;
        InputManager.OnMousePos += OnMousePosInput;
    }

    private void UnsubscribeFromInput()
    {
        InputManager.OnMove -= OnMoveInput;
        InputManager.OnLook -= OnLookInput;
        InputManager.OnDrag -= OnDragInput;
        InputManager.OnZoom -= OnZoomInput;
        InputManager.OnMousePos -= OnMousePosInput;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }

    private void Start() {
        _mainCamera = Camera.main;
    }

    // update
    private void FixedUpdate()
    {
        //Enable CameraControls and gives it a priority list
        cameraMove = DragCameraMove();

        if(cameraMove == Vector3.zero && _dragInput == 0)
            cameraMove = MouseCameraMove(); 

        if(cameraMove == Vector3.zero && _dragInput == 0) 
            cameraMove = KeyboardCameraMove();

        // updates the movement to the camera
        UpdateCameraMove();

        // allowes the player to zoom
        ZoomCamera();
    }

    private Vector3 DragCameraMove()
    {
        if(_dragInput != 0)
        {
            Vector3 cameraMoveDelta = new Vector3(_lookInput.x, 0, _lookInput.y) * (0.01f * _playerSettings.DragSpeed);
            return Vector3.ProjectOnPlane(cameraMoveDelta, Vector3.Cross(transform.right, transform.forward));
        }
        else
            return Vector3.zero;
    }


    private Vector3 MouseCameraMove()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector3 cameraMoveDelta = Vector3.zero;
        bool reset = true;

        if(_mousePosInput.x >= screenSize.x * (1 - _playerSettings.ScreenEdgeOffset))
        {
            cameraMoveDelta.x = 0.1f * _playerSettings.MouseMoveSpeed;
            reset = false;
        }
        else if (_mousePosInput.x <= screenSize.x * _playerSettings.ScreenEdgeOffset)
        {
            cameraMoveDelta.x = -1 * (0.1f * _playerSettings.MouseMoveSpeed);
            reset = false;
        }
        

        if(_mousePosInput.y >= screenSize.y * (1 - _playerSettings.ScreenEdgeOffset))
        {
            cameraMoveDelta.z = 0.1f * _playerSettings.MouseMoveSpeed;
            reset = false;
        }
        else if (_mousePosInput.y <= screenSize.y * _playerSettings.ScreenEdgeOffset)
        {
            cameraMoveDelta.z = -1 * (0.1f * _playerSettings.MouseMoveSpeed);
            reset = false;
        }

        if(reset)
            return Vector3.zero;

        return cameraMoveDelta;
    }


    private Vector3 KeyboardCameraMove()
    {
        if(_moveInput != Vector2.zero)
        {
            Vector3 cameraMoveDelta = new Vector3(_moveInput.x, 0, _moveInput.y) * (0.1f * _playerSettings.KeyboardMoveSpeed);
            return Vector3.ProjectOnPlane(cameraMoveDelta, Vector3.Cross(transform.right, transform.forward));
        }
        else
            return Vector3.zero;
    }


    private void UpdateCameraMove()
    {
        CameraFollowTarget.position = CameraFollowTarget.position + cameraMove;
    }


    private void ZoomCamera()
    {
        if(_zoomInput != 0)
        {
            CameraFollowTarget.position = CameraFollowTarget.position + (_mainCamera.transform.forward * _zoomInput) * _playerSettings.ZoomSpeed;
        }
    }


    // inputs
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _moveInput = Vector2.zero;
        }
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _lookInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _lookInput = Vector2.zero;
        }
    } 

    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _mousePosInput = Vector2.zero;
        }
    } 

    private void OnDragInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _dragInput = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            _dragInput = 0f;
        }
    } 
    
    private void OnZoomInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float scroll  = context.ReadValue<Vector2>().y;

            if(scroll > 0)
                _zoomInput = 1;
            else
                _zoomInput = -1;
            
        }
        else if (context.canceled)
        {
            _zoomInput = 0;
        }
    } 
}
