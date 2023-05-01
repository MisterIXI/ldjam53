using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutputStation : Placeable, IInteractable
{
    [field: SerializeField] public Transform _outputPoint { get; private set; }
    public IInteractable ParentInteractable { get; private set; }
    public GridTile OutputTile { get; private set; }
    [field: SerializeField] public List<List<GridTile>> _pathsToOutput { get; private set; } = new List<List<GridTile>>();
    public override void Initialize()
    {
        base.Initialize();
        OutputTile = HexGrid.GetTile(_outputPoint.position);
        ParentInteractable = transform.parent.GetComponentInParent<IInteractable>();
    }

    public override void PlaceOnTile(GridTile tile)
    {
        _currentTile = tile;
        tile.Placeable = this;
    }

    public void AddPath(List<GridTile> path)
    {
        _pathsToOutput.Add(path);
    }

    public void ClearPaths()
    {
        _pathsToOutput.Clear();
    }

    public void OnInteract()
    {
        ParentInteractable.OnInteract();
    }

    private void OnDestroy()
    {
        if (_currentTile != null && _currentTile.Placeable == this)
        {
            _currentTile.Placeable = null;
        }
    }
}