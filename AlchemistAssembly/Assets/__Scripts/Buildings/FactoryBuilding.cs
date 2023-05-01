using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FactoryBuilding : Placeable, IInteractable
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;
    private MineCartSettings _mineCartSettings => SettingsManager.MineCartSettings;


    [field: SerializeField] private ResourceType _resourceType;         // change those later to incorp it with yanniks class
    private float _outputTime;


    private Sprite _outputSprite;
    private Sprite _input1Sprite;
    private Sprite _input2Sprite;
    private Sprite _input3Sprite;


    [field: SerializeField] private GameObject[] routes;
    private int routeCounter = 0;


    private int recepieInt = 8;

    private float timer = 0;


    void Update()
    {
        if (ReceiverStation.HasAllRessources())
            ProduceOutput();
    }


    private void ProduceOutput()
    {
        if (OutputStation._pathsToOutput.Count != 0 && _resourceType != ResourceType.Empty)  // if there are routes availible then start producing
        {
            timer += Time.deltaTime;

            try
            {
                // if this buildings panel is showing updates the progress slider
                if (HudReferences.Instance.CurrentBuilding == gameObject)
                    HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;
            }
            catch { }

            if (timer >= _outputTime)
            {
                SendOutput();
                timer = 0;
            }
        }
    }


    private int FindRecepie(ResourceType Output)
    {
        if (Output == ResourceType.RedPot)
            HUDManager.Instance.ChangetooltipText(5);
        for (int i = 0; i < _buildingSettings.Recipes.Length - 1; i++)
        {
            if (Output == _buildingSettings.Recipes[i].Output)
                return i;
        }
        return 8;
    }


    private Sprite FindSprite(ResourceType resource)
    {
        if (resource == ResourceType.Shroom)
            return _buildingSettings.ShroomSprite;
        else if (resource == ResourceType.Water)
            return _buildingSettings.WaterSprite;
        else if (resource == ResourceType.Crystal)
            return _buildingSettings.CrystalSprite;
        else if (resource == ResourceType.Honey)
            return _buildingSettings.HoneySprite;
        else if (resource == ResourceType.RedPot)
            return _buildingSettings.RedPotSprite;
        else if (resource == ResourceType.HoneyWater)
            return _buildingSettings.HoneyWaterSprite;
        else if (resource == ResourceType.BluePot)
            return _buildingSettings.BluePotSprite;
        else if (resource == ResourceType.RedHoney)
            return _buildingSettings.RedHoneySprite;
        else if (resource == ResourceType.YellowPot)
            return _buildingSettings.YellowPotSprite;
        else if (resource == ResourceType.CrystalHoney)
            return _buildingSettings.CrystalHoneySprite;
        else if (resource == ResourceType.YellowCrystal)
            return _buildingSettings.YellowCrystalSprite;
        else if (resource == ResourceType.PurplePot)
            return _buildingSettings.PurplePotSprite;
        else
            return _buildingSettings.EmptySprite;
    }


    public void SendOutput()
    {
        Debug.Log("Trying to send Output");
        // check if output spot is free
        Placeable placeable = OutputStation.OutputTile.Placeable;
        if (placeable == null || placeable is not RailEntity || ((RailEntity)placeable).OccupyingMineCart != null)
            return;
        if (OutputStation._pathsToOutput.Count == 0)
            return;
        // resets route 
        if (routeCounter >= (OutputStation._pathsToOutput.Count - 1))
            routeCounter = 0;
        ReceiverStation.ResetResources();
        Debug.Log("Sending Minecart");
        // spawns minecart, gives it the route and sets the resource type
        MineCart minecart = Instantiate(_mineCartSettings.MineCartSubTypesPrefabs[(int)_resourceType], transform.position, Quaternion.identity);
        minecart.Initialize(OutputStation._pathsToOutput[routeCounter], _resourceType);


        routeCounter += 1;
    }




    public void UpdateInputColor()
    {
        // Debug.Log(_input1.ToString() + _input2.ToString() + _input3.ToString());
        if (HudReferences.Instance.CurrentBuilding != gameObject)
            return;
        if (_buildingSettings.Recipes[recepieInt].Input[0] == ResourceType.Empty)
            HudReferences.Instance.InputColor1.GetComponent<Image>().color = _buildingSettings.ColorUnavailable;
        else if (ReceiverStation.TypesStored[0])
            HudReferences.Instance.InputColor1.GetComponent<Image>().color = _buildingSettings.ColorFull;
        else
            HudReferences.Instance.InputColor1.GetComponent<Image>().color = _buildingSettings.ColorEmpty;


        if (_buildingSettings.Recipes[recepieInt].Input[1] == ResourceType.Empty)
            HudReferences.Instance.InputColor2.GetComponent<Image>().color = _buildingSettings.ColorUnavailable;
        else if (ReceiverStation.TypesStored[1])
            HudReferences.Instance.InputColor2.GetComponent<Image>().color = _buildingSettings.ColorFull;
        else
            HudReferences.Instance.InputColor2.GetComponent<Image>().color = _buildingSettings.ColorEmpty;


        if (_buildingSettings.Recipes[recepieInt].Input[2] == ResourceType.Empty)
            HudReferences.Instance.InputColor3.GetComponent<Image>().color = _buildingSettings.ColorUnavailable;
        else if (ReceiverStation.TypesStored[2])
            HudReferences.Instance.InputColor3.GetComponent<Image>().color = _buildingSettings.ColorFull;
        else
            HudReferences.Instance.InputColor3.GetComponent<Image>().color = _buildingSettings.ColorEmpty;
    }

    public void OnInteract() //if building is clicked
    {
        HudReferences.Instance.CurrentBuilding = gameObject;
        HudReferences.Instance.BuildingPanel.SetActive(true);
        HudReferences.Instance.RecepiePanel.SetActive(false);
        HudReferences.Instance.InputPanel.SetActive(true);
        HudReferences.Instance.RecepieButtonPanel.SetActive(true);

        HudReferences.Instance.OutputIconPanel.GetComponent<Image>().sprite = _outputSprite;
        HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;

        UpdateIcons();
        UpdateInputColor();


        // show routes

    }

    public void OnClose()  // if close button is pressed
    {
        HudReferences.Instance.BuildingPanel.SetActive(false);
        HudReferences.Instance.RecepiePanel.SetActive(false);
        HudReferences.Instance.CurrentBuilding = null;

        // hide routes
    }


    public void OnAddRoute()   // if add route button is pressed
    {
        OnClose();
        PlacementController.StartPathFrom(OutputStation.CurrentTile);
    }


    public void OnClearRoutes()    // if clear routes button is pressed
    {
        OutputStation._pathsToOutput.Clear();
    }


    public void OnRecepies()
    {
        HudReferences.Instance.BuildingPanel.SetActive(false);
        HudReferences.Instance.RecepiePanel.SetActive(true);
    }

    public void OnSelectRecepie(ResourceType resource)
    {
        recepieInt = FindRecepie(resource);
        ReceiverStation.SwitchToRessources(_buildingSettings.Recipes[recepieInt].Input);
        _resourceType = resource;

        // find sprte for output and inputs
        _outputSprite = FindSprite(_buildingSettings.Recipes[recepieInt].Output);
        _input1Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[0]);
        _input2Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[1]);
        _input3Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[2]);

        // saves output time
        _outputTime = _buildingSettings.Recipes[recepieInt].Time;
        OnInteract();
    }


    public void UpdateIcons()
    {
        HudReferences.Instance.OutputIconPanel.GetComponent<Image>().sprite = _outputSprite;
        HudReferences.Instance.InputIcon1Panel.GetComponent<Image>().sprite = _input1Sprite;
        HudReferences.Instance.InputIcon2Panel.GetComponent<Image>().sprite = _input2Sprite;
        HudReferences.Instance.InputIcon3Panel.GetComponent<Image>().sprite = _input3Sprite;
    }
}