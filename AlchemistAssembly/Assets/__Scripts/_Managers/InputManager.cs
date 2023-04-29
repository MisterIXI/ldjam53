using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInput _playerInput;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
        SubscribeToInput();
        DontDestroyOnLoad(gameObject);
    }


    public static event Action<CallbackContext> OnLook;
    public static event Action<CallbackContext> OnMove;
    public static event Action<CallbackContext> OnDrag;
    public static event Action<CallbackContext> OnZoom;
    public static event Action<CallbackContext> OnMousePos;
    public static event Action<CallbackContext> OnRotate;
    public static event Action<CallbackContext> OnUi;
    public static event Action<CallbackContext> OnInteract;
    public static event Action<CallbackContext> OnHotkeys;
    public static event Action<CallbackContext> OnMenu;


    private void OnLookInput(CallbackContext context)
    {
        OnLook?.Invoke(context);
    }


    private void OnMoveInput(CallbackContext context)
    {
        OnMove?.Invoke(context);
    }


    private void OnDragInput(CallbackContext context)
    {
        OnDrag?.Invoke(context);
    }


    private void OnZoomInput(CallbackContext context)
    {
        OnZoom?.Invoke(context);
    }


    private void OnMousePosInput(CallbackContext context)
    {
        OnMousePos?.Invoke(context);
    }


    private void OnRotateInput(CallbackContext context)
    {
        OnRotate?.Invoke(context);
    }


    private void OnUiInput(CallbackContext context)
    {
        OnUi?.Invoke(context);
    }


    private void OnInteractInput(CallbackContext context)
    {
        OnInteract?.Invoke(context);
    }


    private void OnHotkeysInput(CallbackContext context)
    {
        OnHotkeys?.Invoke(context);
    }


    private void OnMenuInput(CallbackContext context)
    {
        OnMenu?.Invoke(context);
    }


    private void SubscribeToInput()
    {
        _playerInput.actions["Look"].started += OnLookInput;
        _playerInput.actions["Look"].performed += OnLookInput;
        _playerInput.actions["Look"].canceled += OnLookInput;

        _playerInput.actions["Move"].started += OnMoveInput;
        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].canceled += OnMoveInput;
        
        _playerInput.actions["Drag"].started += OnDragInput;
        _playerInput.actions["Drag"].performed += OnDragInput;
        _playerInput.actions["Drag"].canceled += OnDragInput;
            
        _playerInput.actions["Zoom"].started += OnZoomInput;
        _playerInput.actions["Zoom"].performed += OnZoomInput;
        _playerInput.actions["Zoom"].canceled += OnZoomInput;

        _playerInput.actions["MousePos"].started += OnMousePosInput;
        _playerInput.actions["MousePos"].performed += OnMousePosInput;
        _playerInput.actions["MousePos"].canceled += OnMousePosInput;
        
        _playerInput.actions["Rotate"].started += OnRotateInput;
        _playerInput.actions["Rotate"].performed += OnRotateInput;
        _playerInput.actions["Rotate"].canceled += OnRotateInput;
        
        _playerInput.actions["Ui"].started += OnUiInput;
        _playerInput.actions["Ui"].performed += OnUiInput;
        _playerInput.actions["Ui"].canceled += OnUiInput;

        _playerInput.actions["Interact"].started += OnInteractInput;
        _playerInput.actions["Interact"].performed += OnInteractInput;
        _playerInput.actions["Interact"].canceled += OnInteractInput;
        
        _playerInput.actions["Hotkeys"].started += OnHotkeysInput;
        _playerInput.actions["Hotkeys"].performed += OnHotkeysInput;
        _playerInput.actions["Hotkeys"].canceled += OnHotkeysInput;

        _playerInput.actions["Menu"].started += OnMenuInput;
        _playerInput.actions["Menu"].performed += OnMenuInput;
        _playerInput.actions["Menu"].canceled += OnMenuInput;
    }


    private void UnsubscribeFromInput()
    {
        _playerInput.actions["Look"].started -= OnLookInput;
        _playerInput.actions["Look"].performed -= OnLookInput;
        _playerInput.actions["Look"].canceled -= OnLookInput;

        _playerInput.actions["Move"].started -= OnMoveInput;
        _playerInput.actions["Move"].performed -= OnMoveInput;
        _playerInput.actions["Move"].canceled -= OnMoveInput;
        
        _playerInput.actions["Drag"].started -= OnDragInput;
        _playerInput.actions["Drag"].performed -= OnDragInput;
        _playerInput.actions["Drag"].canceled -= OnDragInput;
                
        _playerInput.actions["Zoom"].started -= OnZoomInput;
        _playerInput.actions["Zoom"].performed -= OnZoomInput;
        _playerInput.actions["Zoom"].canceled -= OnZoomInput;

        _playerInput.actions["MousePos"].started -= OnMousePosInput;
        _playerInput.actions["MousePos"].performed -= OnMousePosInput;
        _playerInput.actions["MousePos"].canceled -= OnMousePosInput;
        
        _playerInput.actions["Rotate"].started -= OnRotateInput;
        _playerInput.actions["Rotate"].performed -= OnRotateInput;
        _playerInput.actions["Rotate"].canceled -= OnRotateInput;
        
        _playerInput.actions["Ui"].started -= OnUiInput;
        _playerInput.actions["Ui"].performed -= OnUiInput;
        _playerInput.actions["Ui"].canceled -= OnUiInput;

        _playerInput.actions["Interact"].started -= OnInteractInput;
        _playerInput.actions["Interact"].performed -= OnInteractInput;
        _playerInput.actions["Interact"].canceled -= OnInteractInput;
        
        _playerInput.actions["Hotkeys"].started -= OnHotkeysInput;
        _playerInput.actions["Hotkeys"].performed -= OnHotkeysInput;
        _playerInput.actions["Hotkeys"].canceled -= OnHotkeysInput;

        _playerInput.actions["Menu"].started -= OnMenuInput;
        _playerInput.actions["Menu"].performed -= OnMenuInput;
        _playerInput.actions["Menu"].canceled -= OnMenuInput;
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            UnsubscribeFromInput();
        }
    }
}