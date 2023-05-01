using UnityEngine;

public class CauldronCrafter : Placeable, IInteractable
{
    public override void Initialize()
    {
        base.Initialize();

    }
    public void OnInteract()
    {
        Debug.Log("Interacted with cauldron");
        PlacementController.StartPathFrom(OutputStation.CurrentTile);
    }
}