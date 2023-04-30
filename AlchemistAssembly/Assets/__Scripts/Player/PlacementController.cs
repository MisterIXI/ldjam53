using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementController : MonoBehaviour
{
    public static event Action<GridTile, GridTile> OnTileHovered;
    public static PlacementController Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;
    public GameObject Cube;

    private Vector2 _mousePosInput;
    public Vector3 mousePos;
    private Plane _groundPlane;

    private Camera _mainCamera;
    private GridTile _lastHoveredTile;
    public static GridTile HoveredTile => Instance._lastHoveredTile;

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
    void Start()
    {
        _mainCamera = Camera.main;
        _groundPlane = new Plane(Vector3.up, 0);
        var railTool = new GameObject("RailTool");
        railTool.AddComponent<LineRenderer>().enabled = false;
        railTool.AddComponent<RailPlacer>().Activate();

    }

    void FixedUpdate()
    {
        ProjectMouse();
        UpdateTileHover();
    }




    private void ProjectMouse()
    {
        float distance;

        Ray ray = _mainCamera.ScreenPointToRay(_mousePosInput);
        if (_groundPlane.Raycast(ray, out distance))
        {
            mousePos = ray.GetPoint(distance);
            // if (_playerSettings.mouseHighlight)
            // {
            //     Cube.SetActive(true);
            //     Cube.transform.position = mousePos;
            // }
            // else
            //     Cube.SetActive(false);
        }
    }

    private void UpdateTileHover()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_mousePosInput);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~LayerMask.GetMask("GridTile")))
        {
            GridTile hoverTile = hit.collider.GetComponentInParent<GridTile>();
            if (hoverTile != _lastHoveredTile)
            {
                OnTileHovered?.Invoke(_lastHoveredTile, hoverTile);
                _lastHoveredTile = hoverTile;
            }
        }
        else
        {
            if (_lastHoveredTile != null)
            {
                OnTileHovered?.Invoke(_lastHoveredTile, null);
                _lastHoveredTile = null;
            }
        }
    }


    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosInput = context.ReadValue<Vector2>();
        }
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
}
