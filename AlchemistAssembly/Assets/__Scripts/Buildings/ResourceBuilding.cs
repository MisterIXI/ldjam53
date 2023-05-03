using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResourceBuilding : Placeable, IInteractable
{
    private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;
    private MineCartSettings _mineCartSettings => SettingsManager.MineCartSettings;
    [field: SerializeField] private ResourceType _resourceType;
    private float _outputTime;
    private Sprite _outputSprite;
    [field: SerializeField] private GameObject[] routes;
    private int routeCounter = 0;


    private float timer = 0;


    private void Start()
    {
        // Finds out what type of Building it is and what time needs to create resource / sprite
        if (_resourceType == ResourceType.Shroom)
        {
            _outputTime = _buildingSettings.ShroomTime;
            _outputSprite = _buildingSettings.ShroomSprite;
        }
        else if (_resourceType == ResourceType.Water)
        {
            _outputTime = _buildingSettings.WaterTime;
            _outputSprite = _buildingSettings.WaterSprite;
        }
        else if (_resourceType == ResourceType.Crystal)
        {
            _outputTime = _buildingSettings.CrystalTime;
            _outputSprite = _buildingSettings.CrystalSprite;
        }
        else if (_resourceType == ResourceType.Honey)
        {
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
        if (OutputStation._pathsToOutput.Count != 0)  // if there are routes availible then start producing
        {
            timer += Time.deltaTime;

            // if this buildings panel is showing updates the progress slider
            if (HudReferences.Instance.CurrentBuilding == gameObject && _outputTime != 0)
                HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;
            else
                HudReferences.Instance.OutputBar.value = 0;

            if (timer >= _outputTime)
            {
                SendOutput();
                timer = 0;
            }
        }
    }


    private void SendOutput()
    {
        // check if output spot is free
        Placeable placeable = OutputStation.OutputTile.Placeable;
        if (placeable == null || placeable is not RailEntity || ((RailEntity)placeable).OccupyingMineCart != null)
            return;
        if (OutputStation._pathsToOutput.Count == 0)
            return;
        // resets route 
        if (routeCounter > (OutputStation._pathsToOutput.Count - 1))
            routeCounter = 0;
        Debug.Log("Sending Minecart");
        // spawns minecart, gives it the route and sets the resource type
        MineCart minecart = Instantiate(_mineCartSettings.MineCartSubTypesPrefabs[(int)_resourceType], transform.position, Quaternion.identity);
        minecart.Initialize(OutputStation._pathsToOutput[routeCounter], _resourceType);

        routeCounter += 1;
    }


    public void OnInteract() //if building is clicked
    {
        foreach (Button button in HudReferences.Instance.BuildingPanel.GetComponentsInChildren<Button>(true))
        {
            button.interactable = false;
        }
        HudReferences.Instance.CurrentBuilding = gameObject;
        HudReferences.Instance.BuildingPanel.SetActive(true);
        HudReferences.Instance.RecepiePanel.SetActive(false);
        HudReferences.Instance.InputPanel.SetActive(false);
        HudReferences.Instance.RecepieButtonPanel.SetActive(false);
        HudReferences.Instance.OutputIconPanel.GetComponent<Image>().sprite = _outputSprite;
        if (_outputTime != 0)
            HudReferences.Instance.OutputBar.value = timer / _outputTime * 100;
        else
            HudReferences.Instance.OutputBar.value = 0;
        StartCoroutine(DelayedButtonActivation());

        // show routes
    }
    private IEnumerator DelayedButtonActivation()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (Button button in HudReferences.Instance.BuildingPanel.GetComponentsInChildren<Button>(true))
        {
            button.interactable = true;
        }
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
}