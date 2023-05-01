using UnityEngine;

[CreateAssetMenu(fileName = "MineCartSettings", menuName = "AlchemistAssembly/MineCartSettings", order = 0)]
public class MineCartSettings : ScriptableObject
{
    [field: Header("Mine Cart Settings")]
    [field: SerializeField][field: Range(0f, 5f)] public float Speed { get; private set; } = 1f;
}