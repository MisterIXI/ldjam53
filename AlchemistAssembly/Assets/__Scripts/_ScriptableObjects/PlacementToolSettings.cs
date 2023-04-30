using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementToolSettings", menuName = "AlchemistAssembly/PlacementToolSettings", order = 0)]
public class PlacementToolSettings : ScriptableObject
{
    [field: Header("Placement Tools")]
    [field: SerializeField] public RessourcePlacementStruct[] PlacementStructs { get; private set; }
    [field: Header("Placement Settings")]
    [field: SerializeField] public float AnimationDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float AnimationUpperLimit { get; private set; } = 1f;
    [field: SerializeField] public float AnimationLowerLimit { get; private set; } = 1f;
    [field: SerializeField] public AnimationCurve AnimationCurve { get; private set; }
    [field: SerializeField] public Color HighlightColor { get; private set; } = new Color(121, 37, 199);
    [field: Header("Materials")]
    [field: SerializeField] public Material NeutralPreviewMaterial { get; private set; }
    [field: SerializeField] public Material ValidPreviewMaterial { get; private set; }
    [field: SerializeField] public Material InvalidPreviewMaterial { get; private set; }
    [field: Header("Prefabs")]
    [field: SerializeField] public RailEntity RailPrefab { get; private set; }
    [field: Header("Destruction Tool Settings")]
    [field: SerializeField] public Color DestructionHighlightColor { get; private set; } = new Color(255, 0, 0);
}

[Serializable]
public struct RessourcePlacementStruct
{
    public TileType BuildingType;
    public Placeable PlaceablePrefab;
}