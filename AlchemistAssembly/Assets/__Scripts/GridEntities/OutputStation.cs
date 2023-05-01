using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutputStation : Placeable, IInteractable
{
    [field: SerializeField] public Transform _outputPoint { get; private set; }
    private IInteractable _parentInteractable;
    public GridTile OutputTile { get; private set; }
    public List<List<GridTile>> _pathsToOutput { get; private set; } = new List<List<GridTile>>();
    public override void Initialize()
    {
        base.Initialize();
        OutputTile = HexGrid.GetTile(_outputPoint.position);
        _parentInteractable = GetComponentInParent<IInteractable>();
    }

    public override void PlaceOnTile(GridTile tile)
    {
        _currentTile = tile;
        tile.Placeable = this;
    }

    public void OnInteract()
    {
        _parentInteractable.OnInteract();
    }

    private void OnDestroy()
    {
        if (_currentTile != null && _currentTile.Placeable == this)
        {
            _currentTile.Placeable = null;
        }
    }
}