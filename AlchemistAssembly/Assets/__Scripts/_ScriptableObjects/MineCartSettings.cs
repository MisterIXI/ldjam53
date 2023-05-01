using UnityEngine;

[CreateAssetMenu(fileName = "MineCartSettings", menuName = "AlchemistAssembly/MineCartSettings", order = 0)]
public class MineCartSettings : ScriptableObject
{
    [field: SerializeField] public MineCart MineCartPrefab { get; private set; }
    [field: SerializeField] public MineCart[] MineCartSubTypesPrefabs { get; private set; }
    [field: Header("Mine Cart Settings")]
    [field: SerializeField][field: Range(0f, 5f)] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public AnimationCurve ScaleCurve { get; private set; }
}