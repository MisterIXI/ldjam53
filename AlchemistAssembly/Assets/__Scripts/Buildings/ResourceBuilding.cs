using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResourceBuilding : MonoBehaviour
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;

    public enum ResourceType   // change those later to incorp it with yanniks class
    {
        Shroom,
        Water,
        Crystal,
        Honey
    }

    UnityEvent m_SendMinecart;

    private ResourceType _resourceType;         // change those later to incorp it with yanniks class
    private float _resourceTime;
    private Sprite _resourceSprite;
    [field: SerializeField] private GameObject[] routes;
    private int routeCoutner = 0;


    private float timer = 0;


    void Start() 
    {
        // Finds out what type of Building it is and what time needs to create resource / sprite
        if(gameObject.tag == "Shroom")
        {
            _resourceType = ResourceType.Shroom;
            _resourceTime = _buildingSettings.ShroomTime;
            _resourceSprite = _buildingSettings.ShroomSprite;
        }
        else if(gameObject.tag == "Water")
        {
            _resourceType = ResourceType.Water;
            _resourceTime = _buildingSettings.WaterTime;
            _resourceSprite = _buildingSettings.WaterSprite;
        }     
        else if(gameObject.tag == "Crystal")
        {
            _resourceType = ResourceType.Crystal;
            _resourceTime = _buildingSettings.CrystalTime;
            _resourceSprite = _buildingSettings.CrystalSprite;
        }
        else if(gameObject.tag == "Honey")
        {
            _resourceType = ResourceType.Honey;
            _resourceTime = _buildingSettings.HoneyTime;
            _resourceSprite = _buildingSettings.HoneySprite;
        }
        else
        {
            Debug.LogError("Building: " + gameObject.name + " has no tag!");
        }
    }


    void Update() 
    {
        // lets the building produce its resource
        ProduceResource();
    }


    private void ProduceResource()
    {
        if(routes != null)  // if there are routes availible then start producing
        {
            timer += Time.deltaTime;

            // if this buildings panel is showing updates the progress slider
            if(_buildingSettings.CurrentBuilding == gameObject)
                _buildingSettings.OutputBar.value = timer / _resourceTime * 100;

            if(timer >= _resourceTime)
            {
                SendResource();
                timer = 0;
            }
        }
    }


    private void SendResource()
    {
        // resets route 
        if(routeCoutner >= (routes.Length -1))
            routeCoutner = 0;

        // invokes an event for minecarts ????
        m_SendMinecart.Invoke(_resourceType, routes[routeCoutner]);              // DAS EVENT FÜR Yannik ? (_resourceType, destination of route)
        routeCoutner += 1;
    }


    private void OnInteract() //if building is clicked
    {
        _buildingSettings.CurrentBuilding = gameObject;
        _buildingSettings.BuildingPanel.SetActive(true);
        _buildingSettings.RecepiePanel.SetActive(false);
        _buildingSettings.InputPanel.SetActive(false);
        _buildingSettings.RecepieButtonPanel.SetActive(false);

        _buildingSettings.OutputIcon.GetComponent<Image>().sprite = _resourceSprite;

        // show routes
    }

    private void OnClose()  // if close button is pressed
    {
        _buildingSettings.BuildingPanel.SetActive(false);
        _buildingSettings.RecepiePanel.SetActive(false);

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