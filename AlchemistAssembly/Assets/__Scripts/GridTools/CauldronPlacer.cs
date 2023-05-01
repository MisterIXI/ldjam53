using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CauldronPlacer : GridTool
{
    [field: SerializeField] public PreviewEntity CauldronPreview { get; private set; }
    [field: SerializeField] public Placeable CauldronPrefab { get; private set; }

    private float _yVelocity;
    public override void Activate()
    {
        base.Activate();
        CauldronPreview.gameObject.SetActive(true);
        CauldronPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);
        ExtraCheck();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        CauldronPreview.gameObject.SetActive(false);
    }
    private void Update()
    {
        float newYAngle = Mathf.SmoothDampAngle(CauldronPreview.transform.eulerAngles.y, PlacementController.Instance.RotationAngle, ref _yVelocity, _settings.PreviewRotationDuration);
        CauldronPreview.transform.eulerAngles = new Vector3(0, newYAngle, 0);
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PlacementController.HoveredTile != null && PlacementController.HoveredTile.Placeable == null)
            {
                if (Placeable.CanPlaceOnTile(PlacementController.HoveredTile, true))
                {
                    PlaceableManager.PlaceObject(CauldronPrefab, PlacementController.HoveredTile, PlacementController.Instance.RotationAngle);
                    HUDManager.Instance.ChangetooltipText(3);
                    ExtraCheck();
                }
            }
        }
    }
    private void ExtraCheck()
    {
        OnNewHover(null, PlacementController.HoveredTile);
    }
    private void OnNewHover(GridTile oldTile, GridTile newTile)
    {
        if (newTile != null)
        {
            CauldronPreview.transform.position = newTile.transform.position;
            if (Placeable.CanPlaceOnTile(newTile, true))
            {
                CauldronPreview.SetPreviewStatus(PreviewStatus.Valid);
            }
            else
            {
                CauldronPreview.SetPreviewStatus(PreviewStatus.Invalid);
            }
        }
    }

    protected override void SubscribeToActions()
    {
        InputManager.OnInteract += OnInteractInput;
        PlacementController.OnTileHovered += OnNewHover;
    }

    protected override void UnsubscribeFromActions()
    {
        InputManager.OnInteract -= OnInteractInput;
        PlacementController.OnTileHovered -= OnNewHover;
    }

    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}