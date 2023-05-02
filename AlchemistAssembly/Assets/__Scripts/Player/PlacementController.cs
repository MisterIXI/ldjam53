using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementController : MonoBehaviour
{
    public GridTool ActiveTool { get; private set; }
    [field: SerializeField] public DefaultTool DefaultTool { get; private set; }
    [field: SerializeField] public PathTool PathTool { get; private set; }
    [field: SerializeField] public RailPlacer RailPlacer { get; private set; }
    [field: SerializeField] public ResourcePlacer ResourcePlacer { get; private set; }
    [field: SerializeField] public CauldronPlacer CauldronPlacer { get; private set; }
    [field: SerializeField] public DestructionTool DestructionTool { get; private set; }
    public static event Action<float> OnRotationChanged;
    public static event Action<GridTile, GridTile> OnTileHovered;
    public static PlacementController Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;
    [HideInInspector] public float RotationAngle = 0f;
    public Vector2 MousePosInput { get; private set; }
    [HideInInspector] public Vector3 mousePos;
    private Plane _groundPlane;
    [field: SerializeField] public LayerMask GridLayer { get; private set; }
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
        // var railTool = new GameObject("RailTool");
        // railTool.AddComponent<LineRenderer>().enabled = false;
        // railTool.AddComponent<RailPlacer>().Activate();
        DefaultTool.Activate();
        ActiveTool = DefaultTool;
        PathTool.gameObject.SetActive(false);
        RailPlacer.gameObject.SetActive(false);
        ResourcePlacer.gameObject.SetActive(false);
        CauldronPlacer.gameObject.SetActive(false);
        DestructionTool.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        ProjectMouse();
        UpdateTileHover();
    }




    private void ProjectMouse()
    {
        float distance;

        Ray ray = _mainCamera.ScreenPointToRay(MousePosInput);
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
        Ray ray = _mainCamera.ScreenPointToRay(MousePosInput);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, GridLayer))
        {
            GridTile hoverTile = hit.collider.GetComponentInParent<GridTile>();
            if (hoverTile != _lastHoveredTile)
            {
                OnTileHovered?.Invoke(_lastHoveredTile, hoverTile);
                _lastHoveredTile?.UnhighlightTile();
                hoverTile?.HighlightTile();
                _lastHoveredTile = hoverTile;
            }
        }
        else
        {
            if (_lastHoveredTile != null)
            {
                OnTileHovered?.Invoke(_lastHoveredTile, null);
                _lastHoveredTile.UnhighlightTile();
                _lastHoveredTile = null;
            }
        }
    }


    private void OnMousePosInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MousePosInput = context.ReadValue<Vector2>();
        }
    }
    private void OnRotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RotationAngle += 60f;
            RotationAngle %= 360f;
            OnRotationChanged?.Invoke(RotationAngle);
        }
    }
    public static void StartPathFrom(GridTile startPoint)
    {
        Instance.PathTool.StartPathFrom(startPoint);
        Instance.SwitchActiveTool(Instance.PathTool);
    }
    public void SwitchActiveTool(GridTool newTool)
    {
        ActiveTool.Deactivate();
        ActiveTool = newTool;
        // switch of newTool for the types (DefaultTool, PathTool, RailPlacer, ResourcePlacer, CauldronPlacer, DestructionTool)
        if (newTool is DefaultTool)
            HUDManager.Instance.UsePathtool(0);
        else if (newTool is PathTool)
            HUDManager.Instance.DisableAllPathtools();
        else if (newTool is RailPlacer)
            HUDManager.Instance.UsePathtool(1);
        else if (newTool is ResourcePlacer)
            HUDManager.Instance.UsePathtool(2);
        else if (newTool is CauldronPlacer)
            HUDManager.Instance.UsePathtool(3);
        else if (newTool is DestructionTool)
            HUDManager.Instance.UsePathtool(4);
        ActiveTool.Activate();
    }
    private void OnHotkeysInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (context.ReadValue<float>())
            {
                case 1:
                    SwitchActiveTool(DefaultTool);
                    break;
                case 2:
                    SwitchActiveTool(RailPlacer);
                    break;
                case 3:
                    SwitchActiveTool(ResourcePlacer);
                    break;
                case 4:
                    SwitchActiveTool(CauldronPlacer);
                    break;
                case 5:
                    SwitchActiveTool(DestructionTool);
                    break;
            }
        }
    }

    public void OnButtonInput(int i)
    {
        switch (i)
        {
            case 1:
                SwitchActiveTool(DefaultTool);
                break;
            case 2:
                SwitchActiveTool(RailPlacer);
                break;
            case 3:
                SwitchActiveTool(ResourcePlacer);
                break;
            case 4:
                SwitchActiveTool(CauldronPlacer);
                break;
            case 5:
                SwitchActiveTool(DestructionTool);
                break;
        }
    }

    private void SubscribeToInput()
    {
        InputManager.OnMousePos += OnMousePosInput;
        InputManager.OnRotate += OnRotateInput;
        InputManager.OnHotkeys += OnHotkeysInput;
    }

    private void UnsubscribeFromInput()
    {
        InputManager.OnMousePos -= OnMousePosInput;
        InputManager.OnRotate -= OnRotateInput;
        InputManager.OnHotkeys -= OnHotkeysInput;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }
}
