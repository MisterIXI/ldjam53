using UnityEngine;

[CreateAssetMenu(fileName = "GridSettings", menuName = "AlchemistAssembly/GridSettings", order = 0)]
public class GridSettings : ScriptableObject
{
    [field: Header("Grid Settings")]
    [field: SerializeField] public Vector2Int GridSize { get; private set; } = new Vector2Int(50, 50);
    [field: SerializeField] public float OuterHexSize { get; private set; } = 2f;
    [field: Header("Prefabs")]
    [field: SerializeField] public GridTile BaseTilePrefab { get; private set; }
}