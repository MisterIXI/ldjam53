using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitchHouse : Placeable
{
    [field: SerializeField] private Transform _blocker1;
    [field: SerializeField] private Transform _blocker2;
    private MineCartSettings _mineCartSettings => SettingsManager.MineCartSettings;

    private int redPotCounter = 0, bluePotCounter = 0, yellowPotCounter = 0, purplePotCounter = 0;


    void Start()
    {
        ResourceType[] AllInputs = new ResourceType[]{ResourceType.Shroom, ResourceType.Water, ResourceType.Crystal, ResourceType.Honey, ResourceType.RedPot,
        ResourceType.HoneyWater, ResourceType.BluePot, ResourceType.RedHoney, ResourceType.YellowPot, ResourceType.CrystalHoney, ResourceType.YellowCrystal,
        ResourceType.PurplePot, ResourceType.Empty};

        ReceiverStation.SwitchToRessources(AllInputs);
    }

    public override void PlaceOnTile(GridTile tile)
    {
        base.PlaceOnTile(tile);
        HexGrid.GetTile(_blocker1.position).Placeable = this;
        HexGrid.GetTile(_blocker2.position).Placeable = this;
    }
    public void AddPoint(ResourceType potionType)
    {
        switch (potionType)
        {
            case ResourceType.RedPot:
                redPotCounter += 1;
                HUDManager.Instance.ChangetooltipText(6);
                break;
            case ResourceType.BluePot:
                bluePotCounter += 1;
                break;
            case ResourceType.YellowPot:
                yellowPotCounter += 1;
                break;
            case ResourceType.PurplePot:
                purplePotCounter += 1;
                break;
        }
        ReceiverStation.ResetResources();
        HUDManager.Instance.redPotionAmount = redPotCounter;
        HUDManager.Instance.bluePotionAmount = bluePotCounter;
        HUDManager.Instance.yellowPotionAmount = yellowPotCounter;
        HUDManager.Instance.yinyangPotionAmount = purplePotCounter;
    }

}
