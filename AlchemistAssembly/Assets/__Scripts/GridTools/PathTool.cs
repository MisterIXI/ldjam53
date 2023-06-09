using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(LineRenderer))]
public class PathTool : GridTool
{
    private GridTile _startTile;
    private LineRenderer _lineRenderer;
    private List<GridTile> _currentPath = new List<GridTile>();
    protected override void Initialize()
    {
        base.Initialize();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
        if (_currentPath == null)
        {
            _currentPath = new List<GridTile>();
        }
    }

    private void OnTileHovered(GridTile oldTile, GridTile newTile)
    {
        if (newTile?.Placeable != null && newTile.Placeable is ReceiverStation receiverStation)
        {
            _currentPath = GetTrackPath(_startTile, newTile);
            _lineRenderer.positionCount = _currentPath.Count;
            for (int i = 0; i < _currentPath.Count; i++)
            {
                _lineRenderer.SetPosition(i, _currentPath[i].transform.position + Vector3.up);
            }
            _lineRenderer.material.color = CheckIfValidTrackPath(_currentPath) ? Color.green : Color.red;
        }
        else
        {
            _currentPath.Clear();
            if (_startTile != null && newTile != null)
            {
                Debug.Log($"Drawing path from {_startTile.TileIndex} to {newTile.TileIndex}");
                var path = HexGrid.GetPathToPos(_startTile.TileIndex, newTile.TileIndex);
                _lineRenderer.positionCount = path.Count;
                for (int i = 0; i < path.Count; i++)
                {
                    _lineRenderer.SetPosition(i, path[i].transform.position + Vector3.up);
                }
                _lineRenderer.material.color = Color.red;
            }
        }
    }

    private bool CheckIfValidTrackPath(List<GridTile> path)
    {
        if (path.Count < 2)
        {
            return false;
        }
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (path[i].Placeable == null || path[i].Placeable is not RailEntity)
            {
                return false;
            }
        }
        return true;
    }
    private List<GridTile> GetTrackPath(GridTile startTile, GridTile endTile)
    {
        List<GridTile> path = new List<GridTile>();
        OutputStation outputStation = startTile.Placeable as OutputStation;
        GridTile currentTile = outputStation.OutputTile;
        GridTile prevTile = startTile;
        HexDirection currentDirection = HexHelper.GetDirection(startTile.TileIndex, currentTile.TileIndex);
        // dijkstra discovery of all railEntities until we reach the endTile
        Queue<GridTile> queue = new Queue<GridTile>();
        HashSet<GridTile> visited = new HashSet<GridTile>();
        visited.Add(startTile);
        Dictionary<GridTile, GridTile> cameFrom = new Dictionary<GridTile, GridTile>();
        cameFrom.Add(startTile, null);
        cameFrom.Add(currentTile, startTile);
        queue.Enqueue(currentTile);
        while (queue.Count > 0)
        {
            currentTile = queue.Dequeue();
            prevTile = cameFrom[currentTile];
            if (currentTile == endTile)
            {
                break;
            }
            currentDirection = HexHelper.GetDirection(prevTile.TileIndex, currentTile.TileIndex);
            for (int i = -1; i < 2; i++)
            {
                HexDirection direction = (HexDirection)(((int)currentDirection + i + 6) % 6);
                Vector2Int neighbourIndex = HexHelper.StepInDirection(currentTile.TileIndex, direction);
                if (HexGrid.IsValidIndex(neighbourIndex))
                {
                    GridTile neighbour = HexGrid.GetTile(neighbourIndex);
                    if (!visited.Contains(neighbour) && neighbour.Placeable != null)
                    {
                        visited.Add(neighbour);
                        if (neighbour.Placeable is RailEntity || neighbour.Placeable is ReceiverStation)
                        {
                            cameFrom.Add(neighbour, currentTile);
                            queue.Enqueue(neighbour);
                        }
                    }
                }
            }
        }
        if (currentTile == endTile)
        {
            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = cameFrom[currentTile];
            }
            path.Add(startTile);
            path.Reverse();
        }
        return path;
    }

    public void StartPathFrom(GridTile startTile)
    {
        _startTile = startTile;
    }
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed && !HudReferences.IsBuildingPanelOpen)
        {
            if (_currentPath != null && _currentPath.Count > 1)
            {
                OutputStation outputStation = _startTile.Placeable as OutputStation;
                outputStation.AddPath(new List<GridTile>(_currentPath));
                PlacementController.Instance.SwitchActiveTool(PlacementController.Instance.DefaultTool);
                if (((ReceiverStation)_currentPath.Last().Placeable).ParentInteractable is FactoryBuilding)
                    HUDManager.Instance.ChangetooltipText(5);
            }
            else
            {
                SoundManager.PlayInvalidActionSound();
            }
        }
    }
 
    protected override void SubscribeToActions()
    {
        PlacementController.OnTileHovered += OnTileHovered;
        InputManager.OnInteract += OnInteractInput;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        _startTile = null;
        _lineRenderer.positionCount = 0;
    }
    protected override void UnsubscribeFromActions()
    {
        PlacementController.OnTileHovered -= OnTileHovered;
        InputManager.OnInteract -= OnInteractInput;
    }
    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}