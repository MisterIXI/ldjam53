using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResourceBuilding : Placeable, IInteractable
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;

    private ResourceType _resourceType;
    private float _outputTime;
    private Sprite _outputSprite;
    [field: SerializeField] private GameObject[] routes;
    private int routeCoutner = 0;


    private float timer = 0;


    private void Start() 
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
        if(routes.Length != 0)  // if there are routes availible then start producing
        {
            timer += Time.deltaTime;

            try
            {
                // if this buildings panel is showing updates the progress slider
                if(HudReferences.Instance.CurrentBuilding == gameObject)
                    HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;
            }catch{}


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
        Debug.Log("Interacting");
        HudReferences.Instance.CurrentBuilding = gameObject;
        HudReferences.Instance.BuildingPanel.SetActive(true);
        HudReferences.Instance.RecepiePanel.SetActive(false);
        HudReferences.Instance.InputPanel.SetActive(false);
        HudReferences.Instance.RecepieButtonPanel.SetActive(false);

        HudReferences.Instance.OutputIconPanel.GetComponent<Image>().sprite = _outputSprite;
        HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;

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
        Debug.Log("Adding Routes");
        HudReferences.Instance.BuildingPanel.SetActive(false);
        HudReferences.Instance.RecepiePanel.SetActive(false);

        // show route tool
        // show routes
        // if building is clicked and route tool
        // -> set route
            // -> OnInteract()
        // if different tool or q -> OnClose()
    }


    public void OnClearRoutes()    // if clear routes button is pressed
    {
        routes = null; // im not sure this works yannik
    }
}