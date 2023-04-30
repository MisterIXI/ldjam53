using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResourceBuilding : MonoBehaviour, IInteractable
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;

    private ResourceType _resourceType;
    private float _outputTime;
    private Sprite _outputSprite;
    [field: SerializeField] private GameObject[] routes;
    private int routeCoutner = 0;


    private float timer = 0;


    void Start() 
    {
        // Finds out what type of Building it is and what time needs to create resource / sprite
        if(gameObject.tag == "Shroom")
        {
            _resourceType = ResourceType.Shroom;
            _outputTime = _buildingSettings.ShroomTime;
            _outputSprite = _buildingSettings.ShroomSprite;
        }
        else if(gameObject.tag == "Water")
        {
            _resourceType = ResourceType.Water;
            _outputTime = _buildingSettings.WaterTime;
            _outputSprite = _buildingSettings.WaterSprite;
        }     
        else if(gameObject.tag == "Crystal")
        {
            _resourceType = ResourceType.Crystal;
            _outputTime = _buildingSettings.CrystalTime;
            _outputSprite = _buildingSettings.CrystalSprite;
        }
        else if(gameObject.tag == "Honey")
        {
            _resourceType = ResourceType.Honey;
            _outputTime = _buildingSettings.HoneyTime;
            _outputSprite = _buildingSettings.HoneySprite;
        }
        else
        {
            Debug.LogError("Building: " + gameObject.name + " has no tag!");
        }
    }


    void Update() 
    {
        // lets the building produce its resource
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


    private void SendOutput()
    {
        // resets route 
        if(routeCoutner >= (routes.Length -1))
            routeCoutner = 0;

        // invokes an event for minecarts ????
        // m_SendMinecart.Invoke(_resourceType, routes[routeCoutner]);              // DAS EVENT FÜR Yannik ? (_resourceType, destination of route)
        routeCoutner += 1;
    }


    public void OnInteract() //if building is clicked
    {
        _buildingSettings.CurrentBuilding = gameObject;
        _buildingSettings.BuildingPanel.SetActive(true);
        _buildingSettings.RecepiePanel.SetActive(false);
        _buildingSettings.InputPanel.SetActive(false);
        _buildingSettings.RecepieButtonPanel.SetActive(false);

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
}