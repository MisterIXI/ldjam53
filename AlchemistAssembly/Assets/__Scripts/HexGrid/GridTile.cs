using UnityEngine;

[SelectionBase]
public class GridTile : MonoBehaviour
{
    public Placeable Placeable;
    public Vector2Int TileIndex { get; private set; }
    public bool IsInitialized => TileIndex != null;

    private MeshRenderer _meshRenderer;
    private Color _defaultColor;
    private PlacementToolSettings _settings;
    public ResourceType ResourceType { get; private set; } = ResourceType.Empty;
    private void Start()
    {
        _settings = SettingsManager.PlacementToolSettings;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultColor = _meshRenderer.material.color;
    }
    public void Initialize(Vector2Int tileIndex)
    {
        TileIndex = tileIndex;
    }

    public void PaintNeighbours()
    {
        var neighbours = HexHelper.GetNeighboursOddQ(TileIndex);
        foreach (var neighbour in neighbours)
        {
            GridTile tile = HexGrid.GetTile(neighbour);
            if (tile != null)
            {
                tile.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
        }
    }
    public void TriggerPath()
    {
        HexTest.FindStreetPath(TileIndex);
    }

    public void HighlightTile()
    {
        if (_settings != null)
            _meshRenderer.material.color = _settings.HighlightColor;
    }

    public void UnhighlightTile()
    {
        if (_settings != null)
            _meshRenderer.material.color = _defaultColor;
    }

    public void SetResourceInfo(Color color, ResourceType resourceType)
    {
        _defaultColor = color;
        _meshRenderer.material.color = color;
        ResourceType = resourceType;
    }
}

public enum TileType
{
    Default,
    Water,
    Mushrooms,
    Crystals,
    Honey
}