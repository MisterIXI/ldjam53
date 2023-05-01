using System;
using System.Linq;
using UnityEngine;
public class ReceiverStation : Placeable, IInteractable
{
    public event Action OnRessourcesConsumed;
    public event Action AllRessources;
    [field: SerializeField] private ResourceType[] _acceptedTypes = new ResourceType[] { ResourceType.Empty };
    private IInteractable _parentInteractable;
    private FactoryBuilding _parentFactory;
    public bool[] TypesStored;

    public override void Initialize()
    {
        base.Initialize();
        TypesStored = new bool[_acceptedTypes.Length];
        _parentInteractable = transform.parent.GetComponent<IInteractable>();
        _parentFactory = transform.parent.GetComponent<FactoryBuilding>();
    }
    public void SwitchToRessources(ResourceType[] types)
    {
        int EmptyCounts = types.Count(x => x == ResourceType.Empty);
        _acceptedTypes = new ResourceType[types.Length - EmptyCounts];
        for (int i = 0; i < _acceptedTypes.Length; i++)
        {
            _acceptedTypes[i] = types[i];
        }
        TypesStored = new bool[_acceptedTypes.Length];
    }
    public bool HasAllRessources()
    {
        for (int i = 0; i < TypesStored.Length; i++)
        {
            if (!TypesStored[i])
            {
                return false;
            }
        }
        return true;
    }
    public bool TryToStoreRessource(ResourceType type)
    {
        for (int i = 0; i < _acceptedTypes.Length; i++)
        {
            if (_acceptedTypes[i] == type && !TypesStored[i])
            {
                TypesStored[i] = true;
                if(_parentFactory != null)
                    _parentFactory.UpdateInputColor();
                return true;
            }
        }
        return false;
    }
    public void OnInteract()
    {
        _parentInteractable.OnInteract();
    }
    public override void PlaceOnTile(GridTile tile)
    {
        _currentTile = tile;
        tile.Placeable = this;
    }

    private void OnDestroy()
    {
        if (_currentTile != null && _currentTile.Placeable == this)
        {
            _currentTile.Placeable = null;
        }
    }

}