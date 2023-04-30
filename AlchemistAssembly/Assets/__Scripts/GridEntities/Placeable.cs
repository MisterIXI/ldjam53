using System.Collections;
using UnityEngine;


public class Placeable : MonoBehaviour
{
    protected GridTile _currentTile;
    private Vector2Int[] _occupiedTiles = new Vector2Int[] { new(0, 0) };
    public virtual Vector2Int[] GetOccupiedTiles() => _occupiedTiles;
    private PlacementToolSettings _settings;
    private void Start()
    {
        _settings = SettingsManager.PlacementToolSettings;
    }
    public virtual void PlaceOnTile(GridTile tile)
    {
        transform.position = tile.transform.position;
        _currentTile = tile;
        transform.parent = tile.transform;
        StartCoroutine(PlacementAnimation());
    }

    protected void FillTiles(Vector2Int startPoint, Vector2Int[] offsets)
    {
        foreach (var tile in offsets)
        {
            GridTile gridTile = HexGrid.GetTile(startPoint + tile);
            if (gridTile != null)
            {
                gridTile.Placeable = this;
            }
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

    public virtual void RotateRight()
    {
        transform.Rotate(Vector3.up, 60f);
    }

    public virtual void RotateLeft()
    {
        transform.Rotate(Vector3.up, -60f);
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
                _currentTile.transform.position = new Vector3(_currentTile.transform.position.x, 0, _currentTile.transform.position.z);
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
                _currentTile.transform.position = new Vector3(_currentTile.transform.position.x, newY, _currentTile.transform.position.z);
            }
            yield return null;
        }
    }
    public virtual void RemoveFromTile()
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
        Destroy(gameObject);
    }
}