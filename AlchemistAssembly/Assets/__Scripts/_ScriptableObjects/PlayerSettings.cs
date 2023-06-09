using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [field: Header ("Debug")]
    [field: SerializeField] public bool mouseHighlight { get; set; } = true;

    [field: Header("Camera")]
    [field: SerializeField] [field: Range(0f, 30f)] public float DragSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 30f)] public float KeyboardMoveSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 30f)] public float MouseMoveSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 1f)] public float ScreenEdgeOffset { get; set; } = 0.05f;
    [field: SerializeField] [field: Range(0f, 30f)] public float ZoomSpeed { get; set; } = 10f;
    [field: SerializeField] [field: Range(-30f, 30f)] public float MinZoom { get; set; } = 3f;
    [field: SerializeField] [field: Range(-30f, 30f)] public float MaxZoom { get; set; } = 25f;
    [field: SerializeField] [field: Range(-200f, 200f)] public float MinPosX { get; set; } = 3f;
    [field: SerializeField] [field: Range(-200f, 200f)] public float MaxPosX { get; set; } = 25f;
    [field: SerializeField] [field: Range(-200f, 200f)] public float MinPosZ { get; set; } = 3f;
    [field: SerializeField] [field: Range(-200f, 200f)] public float MaxPosZ { get; set; } = 25f;
}