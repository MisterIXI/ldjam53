using System;
using UnityEngine;
public class ReceiverStation : Placeable, IInteractable
{
    public event Action OnRessourcesConsumed;
    [field: SerializeField] private ResourceType[] _acceptedTypes = new ResourceType[] { ResourceType.Empty };
    private IInteractable _parentInteractable;
    private bool[] _typesStored;

    public override void Initialize()
    {
        base.Initialize();
        _typesStored = new bool[_acceptedTypes.Length];
        _parentInteractable = GetComponentInParent<IInteractable>();
    }

    public bool TryToStoreRessource(ResourceType type)
    {
        for (int i = 0; i < _acceptedTypes.Length; i++)
        {
            if (_acceptedTypes[i] == type && !_typesStored[i])
            {
                _typesStored[i] = true;
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