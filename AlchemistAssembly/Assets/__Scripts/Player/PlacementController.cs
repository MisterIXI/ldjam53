using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlacementController : MonoBehaviour
{
    public static PlacementController Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;
    public GameObject Cube;

    private Vector2 _mousePosInput;

    private Camera _mainCamera;

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
        InputManager.OnMousePos += OnMousePosInput;
    }

    private void UnsubscribeFromInput()
    {
        InputManager.OnMousePos -= OnMousePosInput;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }

    void Start() 
    {
        _mainCamera = Camera.main;
    }

    void FixedUpdate() 
    {
        ProjectMouse();  
    }

    private void ProjectMouse()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;

        Ray ray = Camera.main.ScreenPointToRay(_mousePosInput);
        if (plane.Raycast(ray, out distance))
        {
            Cube.transform.position = ray.GetPoint(distance);
        }
    }

    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosInput = context.ReadValue<Vector2>();
        }
    } 
}
