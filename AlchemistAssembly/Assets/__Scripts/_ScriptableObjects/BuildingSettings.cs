using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [field: Header("Panel")]
    [field: SerializeField]public GameObject BuildingPanel;
    [field: SerializeField]public GameObject RecepiePanel;
    [field: SerializeField]public GameObject InputPanel;
    [field: SerializeField]public GameObject RecepieButtonPanel;
    [field: SerializeField]public GameObject InputGrey1, InputRed1, InputGreen1;
    [field: SerializeField]public GameObject InputGrey2, InputRed2, InputGreen2;
    [field: SerializeField]public GameObject InputGrey3, InputRed3, InputGreen3;
    [field: SerializeField]public Slider OutputBar;


    [field: Header("Will be overwritten in Script")]
    [field: SerializeField]public GameObject CurrentBuilding;
    [field: SerializeField]public GameObject InputIcon1, InputIcon2, InputIcon3;
    [field: SerializeField]public GameObject OutputIcon;


    [field: Header("Sprites")]
    [field: SerializeField]public Sprite ShroomSprite;
    [field: SerializeField]public Sprite WaterSprite;
    [field: SerializeField]public Sprite CrystalSprite;
    [field: SerializeField]public Sprite HoneySprite;
    [field: SerializeField]public Sprite RedPotSprite;
    [field: SerializeField]public Sprite HoneyWaterSprite;
    [field: SerializeField]public Sprite BluePotSprite;
    [field: SerializeField]public Sprite RedHoneySprite;
    [field: SerializeField]public Sprite YellowPotSprite;
    [field: SerializeField]public Sprite CrystalHoneySprite;
    [field: SerializeField]public Sprite PurplePotSprite;


    [field: Header("Recepies")]
    [field: SerializeField]public ResourceBuilding[] RedPot;
}