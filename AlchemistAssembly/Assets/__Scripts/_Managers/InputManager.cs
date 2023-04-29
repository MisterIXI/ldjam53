using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInput _playerInput;
    public static bool IsKeyboardAndMouse => Instance._playerInput.currentControlScheme != "Gamepad";


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnSchemeChange;
        SubscribeToInput();
        DontDestroyOnLoad(gameObject);
    }


    public static event Action OnControlSchemeChanged;
    public static event Action<CallbackContext> OnLook;
    public static event Action<CallbackContext> OnMove;
    public static event Action<CallbackContext> OnSprint;
    public static event Action<CallbackContext> OnRoll;
    public static event Action<CallbackContext> OnJump;
    public static event Action<CallbackContext> OnAttack;
    public static event Action<CallbackContext> OnBlock;
    public static event Action<CallbackContext> OnSpecialAttack;
    public static event Action<CallbackContext> OnFocus;
    public static event Action<CallbackContext> OnInteract;
    public static event Action<CallbackContext> OnItem;
    public static event Action<CallbackContext> OnRadialMenu;
    public static event Action<CallbackContext> OnMenu;


    private void OnSchemeChange(PlayerInput playerInput)
    {
        OnControlSchemeChanged?.Invoke();
    }


    private void OnLookInput(CallbackContext context)
    {
        OnLook?.Invoke(context);
    }


    private void OnMoveInput(CallbackContext context)
    {
        OnMove?.Invoke(context);
    }


    private void OnSprintInput(CallbackContext context)
    {
        OnSprint?.Invoke(context);
    }


    private void OnRollInput(CallbackContext context)
    {
        OnRoll?.Invoke(context);
    }


    private void OnJumpInput(CallbackContext context)
    {
        OnJump?.Invoke(context);
    }


    private void OnAttackInput(CallbackContext context)
    {
        OnAttack?.Invoke(context);
    }

    private void OnBlockInput(CallbackContext context)
    {
        OnBlock?.Invoke(context);
    }


    private void OnSpecialAttackInput(CallbackContext context)
    {
        OnSpecialAttack?.Invoke(context);
    }


    private void OnFocusInput(CallbackContext context)
    {
        OnFocus?.Invoke(context);
    }


    private void OnInteractInput(CallbackContext context)
    {
        OnInteract?.Invoke(context);
    }


    private void OnItemInput(CallbackContext context)
    {
        OnItem?.Invoke(context);
    }


    private void OnRadialMenuInput(CallbackContext context)
    {
        OnRadialMenu?.Invoke(context);
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
        
        _playerInput.actions["Sprint"].started += OnSprintInput;
        _playerInput.actions["Sprint"].performed += OnSprintInput;
        _playerInput.actions["Sprint"].canceled += OnSprintInput;
            
        _playerInput.actions["Roll"].started += OnRollInput;
        _playerInput.actions["Roll"].performed += OnRollInput;
        _playerInput.actions["Roll"].canceled += OnRollInput;
        
        _playerInput.actions["Jump"].started += OnJumpInput;
        _playerInput.actions["Jump"].performed += OnJumpInput;
        _playerInput.actions["Jump"].canceled += OnJumpInput;

        _playerInput.actions["Attack"].started += OnAttackInput;
        _playerInput.actions["Attack"].performed += OnAttackInput;
        _playerInput.actions["Attack"].canceled += OnAttackInput;
        
        _playerInput.actions["Block"].started += OnBlockInput;
        _playerInput.actions["Block"].performed += OnBlockInput;
        _playerInput.actions["Block"].canceled += OnBlockInput;

        _playerInput.actions["SpecialAttack"].started += OnSpecialAttackInput;
        _playerInput.actions["SpecialAttack"].performed += OnSpecialAttackInput;
        _playerInput.actions["SpecialAttack"].canceled += OnSpecialAttackInput;
        
        _playerInput.actions["Focus"].started += OnFocusInput;
        _playerInput.actions["Focus"].performed += OnFocusInput;
        _playerInput.actions["Focus"].canceled += OnFocusInput;

        _playerInput.actions["Interact"].started += OnInteractInput;
        _playerInput.actions["Interact"].performed += OnInteractInput;
        _playerInput.actions["Interact"].canceled += OnInteractInput;
        
        _playerInput.actions["Item"].started += OnItemInput;
        _playerInput.actions["Item"].performed += OnItemInput;
        _playerInput.actions["Item"].canceled += OnItemInput;

        _playerInput.actions["RadialMenu"].started += OnRadialMenuInput;
        _playerInput.actions["RadialMenu"].performed += OnRadialMenuInput;
        _playerInput.actions["RadialMenu"].canceled += OnRadialMenuInput;

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
        
        _playerInput.actions["Sprint"].started -= OnSprintInput;
        _playerInput.actions["Sprint"].performed -= OnSprintInput;
        _playerInput.actions["Sprint"].canceled -= OnSprintInput;
                
        _playerInput.actions["Roll"].started -= OnRollInput;
        _playerInput.actions["Roll"].performed -= OnRollInput;
        _playerInput.actions["Roll"].canceled -= OnRollInput;
        
        _playerInput.actions["Jump"].started -= OnJumpInput;
        _playerInput.actions["Jump"].performed -= OnJumpInput;
        _playerInput.actions["Jump"].canceled -= OnJumpInput;

        _playerInput.actions["Attack"].started -= OnAttackInput;
        _playerInput.actions["Attack"].performed -= OnAttackInput;
        _playerInput.actions["Attack"].canceled -= OnAttackInput;
        
        _playerInput.actions["Block"].started -= OnBlockInput;
        _playerInput.actions["Block"].performed -= OnBlockInput;
        _playerInput.actions["Block"].canceled -= OnBlockInput;

        _playerInput.actions["SpecialAttack"].started -= OnSpecialAttackInput;
        _playerInput.actions["SpecialAttack"].performed -= OnSpecialAttackInput;
        _playerInput.actions["SpecialAttack"].canceled -= OnSpecialAttackInput;
        
        _playerInput.actions["Focus"].started -= OnFocusInput;
        _playerInput.actions["Focus"].performed -= OnFocusInput;
        _playerInput.actions["Focus"].canceled -= OnFocusInput;

        _playerInput.actions["Interact"].started -= OnInteractInput;
        _playerInput.actions["Interact"].performed -= OnInteractInput;
        _playerInput.actions["Interact"].canceled -= OnInteractInput;
        
        _playerInput.actions["Item"].started -= OnItemInput;
        _playerInput.actions["Item"].performed -= OnItemInput;
        _playerInput.actions["Item"].canceled -= OnItemInput;

        _playerInput.actions["RadialMenu"].started -= OnRadialMenuInput;
        _playerInput.actions["RadialMenu"].performed -= OnRadialMenuInput;
        _playerInput.actions["RadialMenu"].canceled -= OnRadialMenuInput;

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


    [ContextMenu("Debug print controlscheme")]
    private void DebugPrintControlscheme()
    {
        Debug.Log("Is keyboard and mouse: " + IsKeyboardAndMouse);
    }
}