using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSettings", menuName = "Settings/BuildingSettings", order = 0)]
public class BuildingSettings : ScriptableObject
{
    [field: Header("Resource Creation Time")]
    [field: SerializeField] [field: Range(0f, 30f)] public float ShroomTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float WaterTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float CrystalTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float HoneyTime { get; set; } = 5;

    [field: Header("Factory Creation Time")]
    [field: SerializeField] [field: Range(0f, 30f)] public float RedPotTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float HoneyWaterTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float BluePotTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float RedHoneyTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float YellowPotTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float YellowCrystalTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float CrystalHoneyTime { get; set; } = 5;
    [field: SerializeField] [field: Range(0f, 30f)] public float PurplePotTime { get; set; } = 5;
}