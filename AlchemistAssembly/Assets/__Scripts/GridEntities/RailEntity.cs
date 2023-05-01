using UnityEngine;

public class RailEntity : Placeable
{
    [field: SerializeField] public GameObject RailStraight { get; private set; }
    [field: SerializeField] public GameObject RailCorner { get; private set; }
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
                Vector2 direction;
                direction = transform.position - startTile.transform.position;
                if(direction.x < 0)
                {
                    transform.rotation = Quaternion.identity;
                    transform.RotateAround(endTile.transform.position, Vector3.up, -60);
                }else{
                    transform.rotation = Quaternion.identity;
                    transform.RotateAround(endTile.transform.position, Vector3.up, 60);
                }
        }
        else if (startTile != null && endTile == null)
        {

        }
        else
        {

        }
    }
}