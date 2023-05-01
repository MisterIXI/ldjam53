using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudReferences : MonoBehaviour
{
    public static HudReferences Instance { get; private set; }

    [field: SerializeField] public GameObject BuildingPanel;
    [field: SerializeField] public GameObject RecepiePanel;
    [field: SerializeField] public GameObject InputPanel;
    [field: SerializeField] public GameObject RecepieButtonPanel;
    [field: SerializeField] public GameObject InputColor1;
    [field: SerializeField] public GameObject InputColor2;
    [field: SerializeField] public GameObject InputColor3;
    [field: SerializeField] public GameObject InputIcon1Panel, InputIcon2Panel, InputIcon3Panel;
    [field: SerializeField] public GameObject OutputIconPanel;
    [field: SerializeField] public Slider OutputBar;


    [field: SerializeField] public GameObject CurrentBuilding;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
