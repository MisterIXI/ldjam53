using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class RailPlacer : GridTool
{
    private GridTile _startTile;
    private LineRenderer _lineRenderer;
    protected override void Initialize()
    {
        base.Initialize();
        _lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnNewHover(GridTile oldTile, GridTile newTile)
    {
        if (_startTile != null && newTile != null)
        {
            // draw a path from start to newTile
            var path = HexGrid.GetPathToPos(_startTile.TileIndex, newTile.TileIndex);
            _lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                _lineRenderer.SetPosition(i, path[i].transform.position + Vector3.up);
            }
            _lineRenderer.material.color = CheckIfValidPath(path) ? Color.green : Color.red;
        }
    }
    private void PlaceRails()
    {
        if (_startTile == null || PlacementController.HoveredTile == null)
        {
            return;
        }
        var path = HexGrid.GetPathToPos(_startTile.TileIndex, PlacementController.HoveredTile.TileIndex);
        if (!CheckIfValidPath(path))
        {
            SoundManager.PlayInvalidActionSound();
            return;
        }
        if (path.Count == 0)
        {
            Debug.LogWarning("Path is empty");
            return;
        }
        if (path.Count == 1)
        {
            PlaceableManager.PlaceRail(_settings.RailPrefab, path[0], null, null);
            return;
        }
        PlaceableManager.PlaceRail(_settings.RailPrefab, path[0], null, path[1]);
        for (int i = 1; i < path.Count - 1; i++)
        {
            PlaceableManager.PlaceRail(_settings.RailPrefab, path[i], path[i - 1], path[i + 1]);
        }
        PlaceableManager.PlaceRail(_settings.RailPrefab, path[path.Count - 1], path[path.Count - 2], null);
    }
    private bool CheckIfValidPath(List<GridTile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].Placeable != null)
            {
                return false;
            }
        }
        return true;
    }
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GridTile currentTile = PlacementController.HoveredTile;
            if (currentTile != null && currentTile.Placeable == null)
            {
                _startTile = currentTile;
                _lineRenderer.enabled = true;
                _lineRenderer.positionCount = 0;
            }
            else
            {
                // invalid action
                SoundManager.PlayInvalidActionSound();
            }

        }
        if (context.canceled)
        {
            // finish up the path
            if (_startTile != null)
            {
                PlaceRails();
                _startTile = null;
            }
            _lineRenderer.enabled = false;
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