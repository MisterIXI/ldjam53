using UnityEngine;

[CreateAssetMenu(fileName = "GridSettings", menuName = "AlchemistAssembly/GridSettings", order = 0)]
public class GridSettings : ScriptableObject
{
    [field: Header("Grid Settings")]
    [field: SerializeField] public float OuterHexSize { get; private set; } = 2f;
}