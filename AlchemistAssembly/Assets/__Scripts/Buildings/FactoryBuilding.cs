using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FactoryBuilding : MonoBehaviour, IInteractable
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;



    UnityEvent m_SendMinecart;

    private ResourceType _resourceType;         // change those later to incorp it with yanniks class
    private float _outputTime;


    private Sprite _outputSprite;
    private Sprite _input1Sprite;
    private Sprite _input2Sprite;
    private Sprite _input3Sprite;


    [field: SerializeField] private GameObject[] routes;
    private int routeCoutner = 0;

    private ResourceType _input1;
    private ResourceType _input2;
    private ResourceType _input3;

    private int recepieInt;

    private float timer = 0;


    void Update() 
    {

        if(_input1 == _buildingSettings.Recipes[recepieInt].Input[1] && _input2 == _buildingSettings.Recipes[recepieInt].Input[2] && _input3 == _buildingSettings.Recipes[recepieInt].Input[3])
            ProduceOutput();
    }


    private void ProduceOutput()
    {
        if(routes != null)  // if there are routes availible then start producing
        {
            timer += Time.deltaTime;

            // if this buildings panel is showing updates the progress slider
            if(_buildingSettings.CurrentBuilding == gameObject)
                _buildingSettings.OutputBar.value = timer / _outputTime * 100;

            if(timer >= _outputTime)
            {
                SendOutput();
                timer = 0;
            }
        }
    }


    private int FindRecepie(ResourceType Output)
    {
        for(int i = 0; i < _buildingSettings.Recipes.Length -1; i++)
        {
            if(Output == _buildingSettings.Recipes[i].Output)
            return i;  
        }
        return -1;
    }


    private Sprite FindSprite(ResourceType resource)
    {
        if(resource == ResourceType.Shroom)
            return _buildingSettings.ShroomSprite;
        else if(resource == ResourceType.Water)
            return _buildingSettings.WaterSprite;
        else if(resource == ResourceType.Crystal)
            return _buildingSettings.CrystalSprite;
        else if(resource == ResourceType.Honey)
            return _buildingSettings.HoneySprite;
        else if(resource == ResourceType.RedPot)
            return _buildingSettings.RedPotSprite;
        else if(resource == ResourceType.HoneyWater)
            return _buildingSettings.HoneyWaterSprite;
        else if(resource == ResourceType.BluePot)
            return _buildingSettings.BluePotSprite;
        else if(resource == ResourceType.RedHoney)
            return _buildingSettings.RedHoneySprite;
        else if(resource == ResourceType.YellowPot)
            return _buildingSettings.YellowPotSprite;
        else if(resource == ResourceType.CrystalHoney)
            return _buildingSettings.CrystalHoneySprite;
        else if(resource == ResourceType.YellowCrystal)
            return _buildingSettings.YellowCrystalSprite;
        else if(resource == ResourceType.PurplePot)
            return _buildingSettings.PurplePotSprite;
        else
            return _buildingSettings.EmptySprite;
    }


    private void SendOutput()
    {
        // resets route 
        if(routeCoutner >= (routes.Length -1))
            routeCoutner = 0;

        // invokes an event for minecarts ????
        // m_SendMinecart.Invoke(_resourceType, routes[routeCoutner]);              // DAS EVENT FÜR Yannik ? (_resourceType, destination of route)
        routeCoutner += 1;
    }


    private void AcceptInput()
    {

    }

    public void OnInteract() //if building is clicked
    {
        _buildingSettings.CurrentBuilding = gameObject;
        _buildingSettings.BuildingPanel.SetActive(true);
        _buildingSettings.RecepiePanel.SetActive(false);
        _buildingSettings.InputPanel.SetActive(true);
        _buildingSettings.RecepieButtonPanel.SetActive(true);

        _buildingSettings.OutputIconPanel.GetComponent<Image>().sprite = _outputSprite;

        // show routes
    }

    private void OnClose()  // if close button is pressed
    {
        _buildingSettings.BuildingPanel.SetActive(false);
        _buildingSettings.RecepiePanel.SetActive(false);
        _buildingSettings.CurrentBuilding = null;

        // hide routes
    }


    private void OnAddRoute()   // if add route button is pressed
    {
        _buildingSettings.BuildingPanel.SetActive(false);
        _buildingSettings.RecepiePanel.SetActive(false);

        // show route tool
        // show routes
        // if building is clicked and route tool
        // -> set route
            // -> OnInteract()
        // if different tool or q -> OnClose()
    }


    private void OnClearRoutes()    // if clear routes button is pressed
    {
        routes = null; // im not sure this works yannik
    }


    private void OnRecepies()
    {
        _buildingSettings.BuildingPanel.SetActive(false);
        _buildingSettings.RecepiePanel.SetActive(true);
    }

    private void OnSelectRecepie(ResourceType resource)
    {
        recepieInt = FindRecepie(resource);

        // Resets saved inputs that are wrong
        if(_buildingSettings.Recipes[recepieInt].Input[0] != _input1)
            _input1 = ResourceType.Empty;
        if(_buildingSettings.Recipes[recepieInt].Input[1] != _input2)
            _input2 = ResourceType.Empty;
        if(_buildingSettings.Recipes[recepieInt].Input[2] != _input3)
            _input3 = ResourceType.Empty;

        // find sprte for output and inputs
        _outputSprite = FindSprite(_buildingSettings.Recipes[recepieInt].Output);
        _input1Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[0]);
        _input2Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[1]);
        _input3Sprite = FindSprite(_buildingSettings.Recipes[recepieInt].Input[2]);

        // saves output time
        _outputTime = _buildingSettings.Recipes[recepieInt].Time;
    }
}