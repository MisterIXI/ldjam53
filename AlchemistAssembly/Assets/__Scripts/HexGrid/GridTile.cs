using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class GridTile : MonoBehaviour
{
    public Placeable Placeable;
    public Vector2Int TileIndex { get; private set; }
    public bool IsInitialized => TileIndex != null;

    private MeshRenderer _meshRenderer;
    private Material _defaultMaterial;
    private PlacementToolSettings _settings;
    public ResourceType ResourceType { get; private set; } = ResourceType.Empty;

    public void Initialize(Vector2Int tileIndex)
    {
        _settings = SettingsManager.PlacementToolSettings;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
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
            _meshRenderer.material = _settings.HighlightMaterial;
    }

    public void UnhighlightTile()
    {
        if (_settings != null)
            _meshRenderer.material = _defaultMaterial;
    }

    public void SetResourceInfo(Material material, ResourceType resourceType)
    {
        _defaultMaterial = material;
        _meshRenderer.material = material;
        ResourceType = resourceType;
    }

    public void StartAnimation()
    {
        StartCoroutine(FallingAnimation());
    }

    public IEnumerator FallingAnimation()
    {
        Vector3 startPosition = transform.position;
        if (_settings == null)
            _settings = SettingsManager.PlacementToolSettings;
        float startTime = Time.time;
        float endTime = startTime + _settings.AnimationDuration;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / _settings.AnimationDuration;
            float newY = _settings.AnimationCurve.Evaluate(t);
            if (newY >= 0)
                newY *= _settings.AnimationUpperLimit;
            else
                newY *= _settings.AnimationLowerLimit;
            transform.position = startPosition + newY * Vector3.up;
            yield return null;
        }
        transform.position = startPosition;
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