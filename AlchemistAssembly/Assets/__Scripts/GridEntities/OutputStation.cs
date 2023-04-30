using UnityEngine;

public class OutputStation : Placeable
{
    [field: SerializeField] public Transform _outputPoint { get; private set; }
    public GridTile OutputTile { get; private set; }
    public override void Initialize()
    {
        base.Initialize();
        OutputTile = HexGrid.GetTile(_outputPoint.position);
    }

    public override void PlaceOnTile(GridTile tile)
    {
        _currentTile = tile;
        tile.Placeable = this;
    }

    private void OnDestroy()
    {
        if (_currentTile != null && _currentTile.Placeable == this)
        {
            _currentTile.Placeable = null;
        }
    }
}