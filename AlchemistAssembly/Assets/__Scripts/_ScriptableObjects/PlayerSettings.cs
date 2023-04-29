using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [field: Header("Camera")]
    [field: SerializeField] [field: Range(0f, 30f)] public float DragSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 30f)] public float KeyboardMoveSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 30f)] public float MouseMoveSpeed { get; set; } = 10;
    [field: SerializeField] [field: Range(0f, 1f)] public float ScreenEdgeOffset { get; set; } = 0.05f;
}