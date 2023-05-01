using UnityEngine;

public class RailEntity : Placeable
{
    [field: SerializeField] public GameObject RailStraight { get; private set; }
    [field: SerializeField] public GameObject RailCorner { get; private set; }
    [HideInInspector] public MineCart OccupyingMineCart;
    public override void PlaceOnTile(GridTile tile)
    {
        base.PlaceOnTile(tile);
    }

    public void ConnectTiles(GridTile startTile, GridTile endTile)
    {
        RailStraight.SetActive(true);
        if (startTile == null && endTile == null)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (startTile == null && endTile != null)
        {

        }
        else if (startTile != null && endTile == null)
        {

        }
        else
        {

        }
    }
}