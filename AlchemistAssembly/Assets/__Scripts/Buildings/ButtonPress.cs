using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;


    public void OnInteract()
    {
        FactoryBuilding factory = HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>();
        if (factory != null)
        {
            factory.OnInteract();
        }
        else
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<ResourceBuilding>().OnInteract();
        }
    }


    public void OnClose()
    {
        FactoryBuilding factory = HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>();
        if (factory != null)
        {
            factory.OnClose();
        }
        else
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<ResourceBuilding>().OnClose();
        }
    }

    public void OnAddRoute()
    {
        FactoryBuilding factory = HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>();
        if (factory != null)
        {
            factory.OnAddRoute();
        }
        else
        {

            HudReferences.Instance.CurrentBuilding.GetComponent<ResourceBuilding>().OnAddRoute();
        }
    }

    public void OnClearRoutes()
    {
        FactoryBuilding factory = HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>();
        if (factory != null)
        {
            factory.OnClearRoutes();
        }
        else
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<ResourceBuilding>().OnClearRoutes();
        }
    }

    public void OnRecepies()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnRecepies();
        }
        catch
        {
            Debug.LogError("Failed to Open Recipes Panel.");
        }
    }

    public void OnSelectRedPot()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.RedPot);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }

    public void OnSelectRedHoney()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.RedHoney);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }

    public void OnSelectHoneyWater()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.HoneyWater);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }

    public void OnSelectBluePot()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.BluePot);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }

    public void OnSelectYellowPot()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.YellowPot);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }


    public void OnSelectCrystalHoney()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.CrystalHoney);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }

    public void OnSelectYellowCrystal()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.YellowCrystal);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }


    public void OnSelectPurplePot()
    {
        try
        {
            HudReferences.Instance.CurrentBuilding.GetComponent<FactoryBuilding>().OnSelectRecepie(ResourceType.PurplePot);
        }
        catch
        {
            Debug.LogError("Failed to Select Recipes.");
        }
    }
}
