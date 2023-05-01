using System;
using UnityEngine;



public class RessourceProducer : Placeable, IInteractable
{
    public override void Initialize()
    {
        base.Initialize();
    }
    public void OnInteract()
    {
        Debug.Log("Interacted with ressource producer");
    }
    public override void PlaceOnTile(GridTile tile)
    {
        base.PlaceOnTile(tile);
        OutputStation.PlaceOnTile(tile);
    }

}