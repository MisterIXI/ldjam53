using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuildingSettings", menuName = "Settings/BuildingSettings", order = 0)]
public class BuildingSettings : ScriptableObject
{
    [field: Header("Resource Creation Time")]
    [field: SerializeField][field: Range(0f, 30f)] public float ShroomTime { get; set; } = 5;
    [field: SerializeField][field: Range(0f, 30f)] public float WaterTime { get; set; } = 5;
    [field: SerializeField][field: Range(0f, 30f)] public float CrystalTime { get; set; } = 5;
    [field: SerializeField][field: Range(0f, 30f)] public float HoneyTime { get; set; } = 5;


    [field: Header("Sprites")]
    [field: SerializeField] public Sprite EmptySprite;
    [field: SerializeField] public Sprite ShroomSprite;
    [field: SerializeField] public Sprite WaterSprite;
    [field: SerializeField] public Sprite CrystalSprite;
    [field: SerializeField] public Sprite HoneySprite;
    [field: SerializeField] public Sprite RedPotSprite;
    [field: SerializeField] public Sprite HoneyWaterSprite;
    [field: SerializeField] public Sprite BluePotSprite;
    [field: SerializeField] public Sprite RedHoneySprite;
    [field: SerializeField] public Sprite YellowPotSprite;
    [field: SerializeField] public Sprite CrystalHoneySprite;
    [field: SerializeField] public Sprite YellowCrystalSprite;
    [field: SerializeField] public Sprite PurplePotSprite;

    [field: Header("Colors")]
    [field: SerializeField] public Color ColorFull;
    [field: SerializeField] public Color ColorEmpty;
    [field: SerializeField] public Color ColorUnavailable;

    [field: Header("Recipes")]
    [field: SerializeField] public CraftingRecipe[] Recipes;
}

[Serializable]
public struct CraftingRecipe
{
    public ResourceType[] Input;
    public ResourceType Output;
    [Range(0f,20f)]public float Time;
}

public enum ResourceType
{
    Shroom,
    Water,
    Crystal,
    Honey,
    RedPot,
    HoneyWater,
    BluePot,
    RedHoney,
    YellowPot,
    CrystalHoney,
    YellowCrystal,
    PurplePot,
    Empty
}