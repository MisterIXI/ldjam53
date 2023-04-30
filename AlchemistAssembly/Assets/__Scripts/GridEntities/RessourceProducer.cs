using System;
using UnityEngine;



public class RessourceProducer : Placeable
{
    [field: SerializeField] public OutputStation OutputStation { get; private set; }
    public override void Initialize()
    {
        base.Initialize();
        OutputStation.Initialize();
    }

    public override void PlaceOnTile(GridTile tile)
    {
        base.PlaceOnTile(tile);
        OutputStation.PlaceOnTile(tile);
    }

}