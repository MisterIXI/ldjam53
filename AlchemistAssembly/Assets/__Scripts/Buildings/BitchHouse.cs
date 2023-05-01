using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitchHouse : Placeable
{
    private MineCartSettings _mineCartSettings => SettingsManager.MineCartSettings;

    private int redPotCounter = 0, bluePotCounter = 0, yellowPotCounter = 0, purplePotCounter = 0;


    void Start() 
    {
        ResourceType[] AllInputs = new ResourceType[]{ResourceType.Shroom, ResourceType.Water, ResourceType.Crystal, ResourceType.Honey, ResourceType.RedPot, 
        ResourceType.HoneyWater, ResourceType.BluePot, ResourceType.RedHoney, ResourceType.YellowPot, ResourceType.CrystalHoney, ResourceType.YellowCrystal,
        ResourceType.PurplePot, ResourceType.Empty};

        ReceiverStation.SwitchToRessources(AllInputs);
    }

    private void UpdateInputColor() // this function is needed in the RecieverStation Script but unused so i missapropriated it
    {
        for(int i = 0; i < ReceiverStation.TypesStored.Length; i++)
        {
            bool _bool = ReceiverStation.TypesStored[i];
            
            if(_bool)
            {
                if((ResourceType)i == ResourceType.RedPot)
                {                    
                    redPotCounter += 1;
                    HUDManager.Instance.ChangetooltipText(6);
                }
                else if((ResourceType)i == ResourceType.BluePot)
                    bluePotCounter += 1;
                else if((ResourceType)i == ResourceType.YellowPot)
                    yellowPotCounter += 1;
                else if((ResourceType)i == ResourceType.PurplePot)
                    purplePotCounter += 1;

                ReceiverStation.TypesStored[i] = false;
            }   
        }

        HUDManager.Instance.redPotionAmount = redPotCounter;
        HUDManager.Instance.bluePotionAmount = bluePotCounter;
        HUDManager.Instance.yellowPotionAmount = yellowPotCounter;
        HUDManager.Instance.yinyangPotionAmount = purplePotCounter;  
    }
}
