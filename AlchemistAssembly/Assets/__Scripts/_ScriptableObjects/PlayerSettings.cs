using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [field: Header("Debug")]
    [field: SerializeField] public bool DisplayPlayerState { get; set; } = false;
    [field: SerializeField] public bool DisplayMovementStats { get; set; } = false;
    [field: SerializeField] public bool TestBool { get; set; } = false;


    [field: Header("Move")]
    [field: SerializeField] public AnimationCurve MoveCurve { get; set; }
    [field: SerializeField] [field: Range(0f, 10f)] public float MoveCurveMaxTime { get; set; } = 1;
    [field: SerializeField] [field: Range(0f, 100f)] public float MoveSpeed { get; set; } = 12;

    [field: SerializeField] public AnimationCurve SprintCurve { get; set; }
    [field: SerializeField] [field: Range(0f, 10f)] public float SprintCurveMaxTime { get; set; } = 1;
    [field: SerializeField] [field: Range(0f, 100f)] public float SprintSpeedDelta { get; set; } = 5;
    
    [field: Header("Jump")]
    [field: SerializeField] public AnimationCurve JumpCurve { get; set; }
    [field: SerializeField] [field: Range(0f, 10f)] public float JumpCurveMaxTime { get; set; } = 1;
    [field: SerializeField] [field: Range(0f, 100f)] public float JumpPower { get; set; } = 15;
    [field: SerializeField] [field: Range(0f, 100f)] public float GravityForce { get; set; } = 50;
    [field: SerializeField] [field: Range(0f, 10)] public float GroundCheckRange { get; set; } = 2;

    [field: Header("Roll")]
    [field: SerializeField] public AnimationCurve RollCurve { get; set; }
    [field: SerializeField] [field: Range(0f, 10f)] public float RollCurveMaxTime { get; set; } = 1;
    [field: SerializeField] [field: Range(0f, 100f)] public float RollPower { get; set; } = 15;
}