using System;
using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    [field: SerializeField] private PlayerSettings _playerSettings;
    [field: SerializeField] private BuildingSettings _buildingSettings;

    public static PlayerSettings PlayerSettings { get; private set; }
    public static BuildingSettings BuildingSettings { get; private set; }

    public static SettingsManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        PlayerSettings = _playerSettings;
        BuildingSettings = _buildingSettings;
        DontDestroyOnLoad(gameObject);
    }
}