using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;


    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _dragInput;
    private Vector2 _mousePosInput;
    private float _cameraRotateInput;


    private Vector3 cameraMove = Vector3.zero;
    private Camera _mainCamera;


    public Transform CameraFollowTarget;
    public Transform CinemachineObj;

    private bool camlock = false;

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
        InputManager.OnCameraRotate += OnCameraRotateInput;
    }

    private void UnsubscribeFromInput()
    {
        InputManager.OnMove -= OnMoveInput;
        InputManager.OnLook -= OnLookInput;
        InputManager.OnDrag -= OnDragInput;
        InputManager.OnZoom -= OnZoomInput;
        InputManager.OnMousePos -= OnMousePosInput;
        InputManager.OnCameraRotate -= OnCameraRotateInput;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    // update
    private void FixedUpdate()
    {
        if (!HudReferences.IsBuildingPanelOpen)
        {
            //Enable CameraControls and gives it a priority list
            cameraMove = DragCameraMove();

            if(cameraMove == Vector3.zero && _dragInput == 0)
                cameraMove = MouseCameraMove(); 

            if (cameraMove == Vector3.zero && _dragInput == 0)
                cameraMove = KeyboardCameraMove();

            // updates the movement to the camera
            UpdateCameraMove();

            // Allow the player to rotate the camera if you are not draggin the cam
            if (_dragInput == 0)
                RotateCamera();
        }
    }

    private Vector3 DragCameraMove()
    {
        if (_dragInput != 0)
        {
            Vector3 cameraMoveDelta = new Vector3(_lookInput.x, 0, _lookInput.y) * (0.01f * _playerSettings.DragSpeed);
            cameraMoveDelta = CameraFollowTarget.transform.TransformDirection(cameraMoveDelta);
            return Vector3.ProjectOnPlane(-cameraMoveDelta, Vector3.Cross(transform.right, transform.forward));
        }
        else
            return Vector3.zero;
    }


    private Vector3 MouseCameraMove()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector3 cameraMoveDelta = Vector3.zero;
        bool reset = true;

        if (_mousePosInput.x >= screenSize.x * (1 - _playerSettings.ScreenEdgeOffset))
        {
            cameraMoveDelta.x = 0.2f * _playerSettings.MouseMoveSpeed;
            reset = false;
        }
        else if (_mousePosInput.x <= screenSize.x * _playerSettings.ScreenEdgeOffset)
        {
            cameraMoveDelta.x = -1 * (0.2f * _playerSettings.MouseMoveSpeed);
            reset = false;
        }


        if (_mousePosInput.y >= screenSize.y * (1 - _playerSettings.ScreenEdgeOffset))
        {
            cameraMoveDelta.z = 0.2f * _playerSettings.MouseMoveSpeed;
            reset = false;
        }
        else if (_mousePosInput.y <= screenSize.y * _playerSettings.ScreenEdgeOffset)
        {
            cameraMoveDelta.z = -1 * (0.2f * _playerSettings.MouseMoveSpeed);
            reset = false;
        }

        if (reset)
            return Vector3.zero;

        cameraMoveDelta = CameraFollowTarget.transform.TransformDirection(cameraMoveDelta);
        return Vector3.ProjectOnPlane(cameraMoveDelta, Vector3.Cross(transform.right, transform.forward));
    }


    private Vector3 KeyboardCameraMove()
    {
        if (_moveInput != Vector2.zero)
        {
            Vector3 cameraMoveDelta = new Vector3(_moveInput.x, 0, _moveInput.y) * (0.2f * _playerSettings.KeyboardMoveSpeed);
            cameraMoveDelta = CameraFollowTarget.transform.TransformDirection(cameraMoveDelta);
            return Vector3.ProjectOnPlane(cameraMoveDelta, Vector3.Cross(transform.right, transform.forward));
        }
        else
            return Vector3.zero;
    }


    private void UpdateCameraMove()
    {
        Vector3 newCamPos = CameraFollowTarget.position + cameraMove;
        float camPosX = Mathf.Clamp(newCamPos.x, _playerSettings.MinPosX, _playerSettings.MaxPosX);
        float camPosZ = Mathf.Clamp(newCamPos.z, _playerSettings.MinPosZ, _playerSettings.MaxPosZ);

        CameraFollowTarget.position = new Vector3(camPosX, newCamPos.y, camPosZ);
    }


    private void ZoomCamera(float _zoomInput)
    {
        if (_zoomInput != 0)
        {
            Vector3 newCamZoom = CameraFollowTarget.position + (_mainCamera.transform.forward * _zoomInput) * _playerSettings.ZoomSpeed;
            float newCamZoomY = newCamZoom.y;
            newCamZoomY = Mathf.Clamp(newCamZoom.y, _playerSettings.MinZoom, _playerSettings.MaxZoom);

            if (newCamZoom.y == newCamZoomY)
                CameraFollowTarget.position = newCamZoom;
        }
    }

    private void RotateCamera()
    {
        if (_cameraRotateInput != 0)
        {
            var camRotation = CinemachineObj.eulerAngles;
            camRotation.y += _lookInput.x;

            CinemachineObj.eulerAngles = camRotation;

            var targetRotation = CameraFollowTarget.eulerAngles;
            targetRotation.y += _lookInput.x;

            CameraFollowTarget.eulerAngles = targetRotation;
        }
    }


    public void LockCameraControls(bool _bool)
    {
        camlock = _bool;
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
    }

    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosInput = context.ReadValue<Vector2>();
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
            float _zoomInput;
            float scroll = context.ReadValue<Vector2>().y;

            if (scroll > 0)
                _zoomInput = 1;
            else
                _zoomInput = -1;

            ZoomCamera(_zoomInput);
        }
    }

    private void OnCameraRotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _cameraRotateInput = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            _cameraRotateInput = 0f;
        }
    }
}
