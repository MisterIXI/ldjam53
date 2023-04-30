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



    [field: Header("Panel")]
    [field: SerializeField] public GameObject BuildingPanel;
    [field: SerializeField] public GameObject RecepiePanel;
    [field: SerializeField] public GameObject InputPanel;
    [field: SerializeField] public GameObject RecepieButtonPanel;
    [field: SerializeField] public GameObject InputGrey1, InputRed1, InputGreen1;
    [field: SerializeField] public GameObject InputGrey2, InputRed2, InputGreen2;
    [field: SerializeField] public GameObject InputGrey3, InputRed3, InputGreen3;
    [field: SerializeField] public GameObject InputIcon1Panel, InputIcon2Panel, InputIcon3Panel;
    [field: SerializeField] public GameObject OutputIconPanel;
    [field: SerializeField] public Slider OutputBar;


    [field: Header("Will be overwritten in Script")]
    [field: SerializeField] public GameObject CurrentBuilding;


    [field: Header("Sprites")]
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
    Empty,
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
    PurplePot
}