using System;
using System.Collections.Generic;
using UnityEngine;


public class PlaceableManager : MonoBehaviour
{
    private Queue<PlaceableInfo> _placeableQueue = new Queue<PlaceableInfo>();
    public static PlaceableManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (_placeableQueue.Count > 0)
        {
            var currentPair = _placeableQueue.Dequeue();
            var newEntity = Instantiate(currentPair.placeable, currentPair.tile.transform.position, Quaternion.identity);
            if (newEntity is RailEntity railEntity)
            {
                railEntity.PlaceOnTile(currentPair.tile);
                railEntity.ConnectTiles(currentPair.startTile, currentPair.endTile);
            }
            else
            {
                newEntity.transform.rotation = Quaternion.Euler(0f, currentPair.Angle, 0f);
                newEntity.PlaceOnTile(currentPair.tile);
            }
        }
    }

    public static void PlaceObject(Placeable placeablePrefab, GridTile tile, float angle = 0f)
    {
        Instance._placeableQueue.Enqueue(new PlaceableInfo { placeable = placeablePrefab, tile = tile, Angle = angle });
    }
    public static void PlaceRail(Placeable placeablePrefab, GridTile tile, GridTile startTile, GridTile endTile)
    {
        Debug.Log("Placing Rail");
        Instance._placeableQueue.Enqueue(new PlaceableInfo { placeable = placeablePrefab, tile = tile, startTile = startTile, endTile = endTile });
    }
}

public struct PlaceableInfo
{
    public Placeable placeable;
    public float Angle;
    public GridTile tile;
    public GridTile startTile;
    public GridTile endTile;
}