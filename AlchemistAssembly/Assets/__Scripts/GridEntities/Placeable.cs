using System.Collections;
using UnityEngine;


public class Placeable : MonoBehaviour
{
    protected GridTile _currentTile;
    private Vector2Int[] _occupiedTiles = new Vector2Int[] { new(0, 0) };
    [field: SerializeField] public bool IsSevenTiles { get; private set; }
    [field: SerializeField] public OutputStation OutputStation { get; private set; }
    [field: SerializeField] public ReceiverStation ReceiverStation { get; private set; }
    public virtual Vector2Int[] GetOccupiedTiles() => _occupiedTiles;
    private PlacementToolSettings _settings;
    private void Start()
    {
        _settings = SettingsManager.PlacementToolSettings;
        Initialize();
    }

    public virtual void Initialize() { }
    public virtual void PlaceOnTile(GridTile tile)
    {
        transform.position = tile.transform.position;
        _currentTile = tile;
        transform.parent = tile.transform;
        FillTiles(tile.TileIndex, GetOccupiedTiles());
        StartCoroutine(PlacementAnimation());
    }

    protected void FillTiles(Vector2Int startPoint, Vector2Int[] offsets)
    {
        _currentTile.Placeable = this;
        if (IsSevenTiles)
        {
            bool isEven = _currentTile.TileIndex.x % 2 == 0;
            if (OutputStation != null)
            {
                GridTile outputTile = HexGrid.GetTile(OutputStation.transform.position);
                OutputStation.PlaceOnTile(outputTile);
            }
            if (ReceiverStation != null)
            {
                GridTile receiverTile = HexGrid.GetTile(ReceiverStation.transform.position);
                ReceiverStation.PlaceOnTile(receiverTile);
            }
            // up and down
            CheckToFillTile(startPoint + new Vector2Int(0, 1));
            CheckToFillTile(startPoint + new Vector2Int(0, -1));
            if (isEven)
            {
                // up left and down left
                CheckToFillTile(startPoint + new Vector2Int(-1, -1));
                CheckToFillTile(startPoint + new Vector2Int(-1, 0));
                // up right and down right
                CheckToFillTile(startPoint + new Vector2Int(1, -1));
                CheckToFillTile(startPoint + new Vector2Int(1, 0));
            }
            else
            {
                // up left and down left
                CheckToFillTile(startPoint + new Vector2Int(-1, 0));
                CheckToFillTile(startPoint + new Vector2Int(-1, 1));
                // up right and down right
                CheckToFillTile(startPoint + new Vector2Int(1, 0));
                CheckToFillTile(startPoint + new Vector2Int(1, 1));
            }
        }
    }

    private void CheckToFillTile(Vector2Int index)
    {
        GridTile tile = HexGrid.GetTile(index);
        if (tile != OutputStation._currentTile && tile != ReceiverStation._currentTile)
        {
            tile.Placeable = this;
        }
    }

    public virtual void HoverOnTile(GridTile tile)
    {
        transform.position = tile.transform.position;

    }

    public bool CanPlaceOnTile(GridTile tile)
    {
        foreach (var offset in GetOccupiedTiles())
        {
            GridTile gridTile = HexGrid.GetTile(tile.TileIndex + offset);
            if (gridTile == null || gridTile.Placeable != null)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator PlacementAnimation()
    {
        if (_settings == null)
            _settings = SettingsManager.PlacementToolSettings;
        float startTime = Time.time;
        float endTime = startTime + _settings.AnimationDuration;
        bool hasTouchedGround = false;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / _settings.AnimationDuration;
            float newY = _settings.AnimationCurve.Evaluate(t);
            if (newY >= 0)
                newY *= _settings.AnimationUpperLimit;
            else
                newY *= _settings.AnimationLowerLimit;
            if (newY > 0)
            {
                transform.localPosition = newY * Vector3.up;
                foreach (var offset in GetOccupiedTiles())
                {
                    GridTile gridTile = HexGrid.GetTile(_currentTile.TileIndex + offset);
                    gridTile.transform.position = new Vector3(gridTile.transform.position.x, 0, gridTile.transform.position.z);
                }
            }
            else
            {
                if (!hasTouchedGround)
                {
                    //TODO: Play sound
                    //TODO: Play particle effect
                    hasTouchedGround = true;
                }
                transform.localPosition = Vector3.zero;
                foreach (var offset in GetOccupiedTiles())
                {
                    GridTile gridTile = HexGrid.GetTile(_currentTile.TileIndex + offset);
                    gridTile.transform.position = new Vector3(gridTile.transform.position.x, newY, gridTile.transform.position.z);
                }
            }
            yield return null;
        }
    }
    public virtual void RemoveFromTile(bool destroyObject = true)
    {
        foreach (var offset in GetOccupiedTiles())
        {
            GridTile gridTile = HexGrid.GetTile(_currentTile.TileIndex + offset);
            if (gridTile != null && gridTile.Placeable == this)
            {
                gridTile.Placeable = null;
            }
            else
            {
                Debug.LogError("Tile " + gridTile + " is not occupied by " + this);
            }
        }
        _currentTile = null;
        if (destroyObject)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_currentTile != null)
        {
            RemoveFromTile(false);
        }
    }
}