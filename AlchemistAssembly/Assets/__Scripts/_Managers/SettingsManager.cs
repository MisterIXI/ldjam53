using System;
using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    [field: SerializeField] private PlayerSettings _playerSettings;
    [field: SerializeField] private PlacementToolSettings _placementToolSettings;
    [field: SerializeField] private GridSettings _gridSettings;
    [field:SerializeField] private BuildingSettings _buildingSettings;

    public static PlayerSettings PlayerSettings => Instance._playerSettings;
    public static PlacementToolSettings PlacementToolSettings => Instance._placementToolSettings;
    public static GridSettings GridSettings => Instance._gridSettings;
    public static BuildingSettings BuildingSettings => Instance._buildingSettings;
    public static SettingsManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}