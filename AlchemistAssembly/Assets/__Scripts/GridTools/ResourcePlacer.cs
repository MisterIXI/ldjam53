using UnityEngine;

using UnityEngine.InputSystem;
public class ResourcePlacer : GridTool
{
    [field: SerializeField] public PreviewEntity DefaultPreview { get; private set; }
    [field: SerializeField] public PreviewEntity MushroomPreview { get; private set; }
    [field: SerializeField] public PreviewEntity WaterPreview { get; private set; }
    [field: SerializeField] public PreviewEntity CrystalPreview { get; private set; }
    [field: SerializeField] public PreviewEntity HoneyPreview { get; private set; }

    [field: SerializeField] public Placeable MushroomPrefab { get; private set; }
    [field: SerializeField] public Placeable WaterPrefab { get; private set; }
    [field: SerializeField] public Placeable CrystalPrefab { get; private set; }
    [field: SerializeField] public Placeable HoneyPrefab { get; private set; }
    private PreviewEntity _currentPreview => _currentResourceIndex switch
    {
        0 => DefaultPreview,
        1 => MushroomPreview,
        2 => WaterPreview,
        3 => CrystalPreview,
        4 => HoneyPreview,
        _ => null
    };
    private Placeable _currentPlaceable => _currentResourceIndex switch
    {
        0 => null,
        1 => MushroomPrefab,
        2 => WaterPrefab,
        3 => CrystalPrefab,
        4 => HoneyPrefab,
        _ => null
    };
    private int _currentResourceIndex = 0;
    private float _yVelocity;
    public override void Activate()
    {
        base.Activate();
        SwitchToResource(0);
        CheckForResource(PlacementController.HoveredTile);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        SwitchToResource(0);
        DefaultPreview.gameObject.SetActive(false);
    }
    private void SwitchToResource(int index)
    {
        _currentResourceIndex = index;
        DefaultPreview.gameObject.SetActive(false);
        MushroomPreview.gameObject.SetActive(false);
        WaterPreview.gameObject.SetActive(false);
        CrystalPreview.gameObject.SetActive(false);
        HoneyPreview.gameObject.SetActive(false);
        switch (index)
        {
            case 0:
                DefaultPreview.gameObject.SetActive(true);
                DefaultPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
                break;
            case 1:
                MushroomPreview.gameObject.SetActive(true);
                MushroomPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
                break;
            case 2:
                WaterPreview.gameObject.SetActive(true);
                WaterPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
                break;
            case 3:
                CrystalPreview.gameObject.SetActive(true);
                CrystalPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
                break;
            case 4:
                HoneyPreview.gameObject.SetActive(true);
                HoneyPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
                break;
        }
    }
    private void CheckForResource(GridTile tile)
    {
        if (tile == null)
            return;
        bool hasResource = true;
        int resource = (int)tile.ResourceType;
        var neighbours = HexHelper.GetNeighboursOddQ(tile.TileIndex);
        foreach (var neighbour in neighbours)
        {
            if (!HexGrid.IsValidIndex(neighbour))
            {
                hasResource = false;
                break;
            }
            if (resource != (int)HexGrid.GetTile(neighbour).ResourceType)
            {
                hasResource = false;
                break;
            }
        }
        resource++;
        if (resource < 1 || resource > 4)
            hasResource = false;
        if (hasResource)
        {
            SwitchToResource(resource);
        }
        else
        {
            SwitchToResource(0);
        }
        _currentPreview.transform.position = tile.transform.position;
        if (Placeable.CanPlaceOnTile(tile, true))
        {
            _currentPreview.SetPreviewStatus(PreviewStatus.Valid);
        }
        else
        {
            _currentPreview.SetPreviewStatus(PreviewStatus.Invalid);
        }
    }


    private void Update()
    {
        float newYAngle = Mathf.SmoothDampAngle(_currentPreview.transform.eulerAngles.y, PlacementController.Instance.RotationAngle, ref _yVelocity, _settings.PreviewRotationDuration);
        _currentPreview.transform.eulerAngles = new Vector3(0, newYAngle, 0);
    }
    private void OnNewHover(GridTile oldTile, GridTile newTile)
    {
        if (newTile != null)
            CheckForResource(newTile);
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GridTile tile = PlacementController.HoveredTile;
            CheckForResource(tile);
            if (_currentResourceIndex > 0 && _currentResourceIndex < 5)
            {
                PlaceableManager.PlaceObject(_currentPlaceable, PlacementController.HoveredTile, PlacementController.Instance.RotationAngle);
                if (_currentResourceIndex == 1)
                    HUDManager.Instance.ChangetooltipText(2);
                if (_currentResourceIndex == 2)
                    HUDManager.Instance.ChangetooltipText(1);
                CheckForResource(tile);
            }
        }
    }

    protected override void SubscribeToActions()
    {
        PlacementController.OnTileHovered += OnNewHover;
        InputManager.OnInteract += OnInteractInput;
    }

    protected override void UnsubscribeFromActions()
    {
        PlacementController.OnTileHovered -= OnNewHover;
        InputManager.OnInteract -= OnInteractInput;
    }
    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}