using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitchHouse : Placeable
{
    [field: SerializeField] private Transform _blocker1;
    [field: SerializeField] private Transform _blocker2;
    private MineCartSettings _mineCartSettings => SettingsManager.MineCartSettings;

    public int redPotCounter { get; private set; } = 0;
    public int bluePotCounter { get; private set; } = 0;
    public int yellowPotCounter { get; private set; } = 0;
    public int purplePotCounter { get; private set; } = 0;


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
                HUDManager.Instance.AddPotionRed();
                break;
            case ResourceType.BluePot:
                bluePotCounter += 1;
                HUDManager.Instance.AddPotionBlue();
                break;
            case ResourceType.YellowPot:
                yellowPotCounter += 1;
                HUDManager.Instance.AddPotionYellow();
                break;
            case ResourceType.PurplePot:
                purplePotCounter += 1;
                HUDManager.Instance.AddPotionYinYang();
                break;
        }
        ReceiverStation.ResetResources();
    }

}
