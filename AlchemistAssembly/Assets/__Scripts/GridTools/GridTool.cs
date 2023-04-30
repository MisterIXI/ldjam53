using System;
using UnityEngine;


public abstract class GridTool : MonoBehaviour
{
    public static event Action OnInvalidPlacement;
    protected PlacementToolSettings _settings;

    private void Start()
    {
        _settings = SettingsManager.PlacementToolSettings;
        Initialize();
    }
    protected virtual void Initialize() { }
    public virtual void Activate() => SubscribeToActions();
    protected abstract void SubscribeToActions();

    public virtual void Deactivate() => UnsubscribeFromActions();
    protected abstract void UnsubscribeFromActions();
}