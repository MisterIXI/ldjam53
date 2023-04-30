// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Events;

// public class FactoryBuilding : MonoBehaviour, IInteractable
// {
//     private BuildingSettings _buildingSettings => SettingsManager.BuildingSettings;

//     public enum ProductionType   // change those later to incorp it with yanniks class
//     {
//         Empty,
//         RedPot,
//         HoneyWater,
//         BluePot,
//         RedHoney,
//         YellowPot,
//         CrystalHoney,
//         PurplePot
//     }

//     [field: SerializeField] private GameObject[] routes;
//     private int routeCoutner = 0;

//     private float timer = 0;

//     private ProductionType _currProduction;

//     void Start() 
//     {
//         _currProduction = ProductionType.Empty;
//     }

//     void Update() 
//     {
//         // lets the building produce
//         ProduceOutput();
//     }


//     private void ProduceOutput()
//     {
//         if(routes != null && _currProduction != ProductionType.Empty)
//         {
//             timer += Time.deltaTime;

//             // if this buildings panel is showing updates the progress slider
//             if(_buildingSettings.CurrentBuilding == gameObject)
//                 _buildingSettings.OutputBar.value = timer / _outputTime * 100;

//             if(timer >= _outputTime)
//             {
//                 SendOutput();
//                 timer = 0;
//             }
//         }
//     }


//     private void SendOutput()
//     {
//         // resets route 
//         if(routeCoutner >= (routes.Length -1))
//             routeCoutner = 0;

//         // invokes an event for minecarts ????
//         // m_SendMinecart.Invoke(_resourceType, routes[routeCoutner]);              // DAS EVENT FÜR Yannik ? (_resourceType, destination of route)
//         routeCoutner += 1;
//     }


//     public void OnInteract() //if building is clicked
//     {
//         _buildingSettings.CurrentBuilding = gameObject;
//         _buildingSettings.BuildingPanel.SetActive(true);
//         _buildingSettings.RecepiePanel.SetActive(false);

//         _buildingSettings.OutputIcon.GetComponent<Image>().sprite = _outputSprite;

//         // show routes
//     }

//     private void OnClose()  // if close button is pressed
//     {
//         _buildingSettings.BuildingPanel.SetActive(false);
//         _buildingSettings.RecepiePanel.SetActive(false);

//         // hide routes
//     }


//     private void OnAddRoute()   // if add route button is pressed
//     {
//         _buildingSettings.BuildingPanel.SetActive(false);
//         _buildingSettings.RecepiePanel.SetActive(false);

//         // show route tool
//         // show routes
//         // if building is clicked and route tool
//         // -> set route
//             // -> OnInteract()
//         // if different tool or q -> OnClose()
//     }


//     private void OnClearRoutes()    // if clear routes button is pressed
//     {
//         routes = null; // im not sure this works yannik
//     }
// }