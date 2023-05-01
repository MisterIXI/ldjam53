using UnityEngine;

public class RailEntity : Placeable
{
    [field: SerializeField] public GameObject RailStraight { get; private set; }
    [field: SerializeField] public GameObject RailCorner { get; private set; }
    public MineCart OccupyingMineCart;
    public override void PlaceOnTile(GridTile tile)
    {
        base.PlaceOnTile(tile);
    }

    public void ConnectTiles(GridTile startTile, GridTile endTile)
    {
        if (startTile == null && endTile == null)
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(1, 1, 1);
            RailStraight.SetActive(true);
            RailCorner.SetActive(false);
        }
        else if (startTile == null && endTile != null)
        {
            HexDirection direction = HexHelper.GetDirection(_currentTile.TileIndex, endTile.TileIndex);
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)direction * 60, 0));
            transform.localScale = new Vector3(1, 1, 1);
            RailStraight.SetActive(true);
            RailCorner.SetActive(false);
        }
        else if (startTile != null && endTile == null)
        {
            HexDirection direction = HexHelper.GetDirection(_currentTile.TileIndex, startTile.TileIndex);
            transform.rotation = Quaternion.Euler(new Vector3(0, (int)direction * 60, 0));
            transform.localScale = new Vector3(1, 1, 1);
            RailStraight.SetActive(true);
            RailCorner.SetActive(false);
        }
        else
        {
            // Debug.Log($"RailEntity: ConnectTiles: startTile: {startTile}, endTile: {endTile}");
            HexDirection startDirection = HexHelper.GetDirection(_currentTile.TileIndex, startTile.TileIndex);
            HexDirection endDirection = HexHelper.GetDirection(_currentTile.TileIndex, endTile.TileIndex);
            // get steps in mod 6 to get the difference in direction
            int steps = (int)endDirection - (int)startDirection;
            // if the difference is negative, add 6 to get the positive difference
            if (steps < 0)
            {
                steps += 6;
            }
            switch (steps)
            {
                case 3: // straight
                    transform.rotation = Quaternion.Euler(new Vector3(0, (int)startDirection * 60, 0));
                    transform.localScale = new Vector3(1, 1, 1);
                    RailStraight.SetActive(true);
                    RailCorner.SetActive(false);
                    break;
                case 4: // right turn
                    transform.rotation = Quaternion.Euler(new Vector3(0, (int)startDirection * 60, 0));
                    transform.localScale = new Vector3(1, 1, 1);
                    RailStraight.SetActive(false);
                    RailCorner.SetActive(true);
                    break;
                case 2: // left turn
                    transform.rotation = Quaternion.Euler(new Vector3(0, (int)startDirection * 60, 0));
                    transform.localScale = new Vector3(-1, 1, 1);
                    RailStraight.SetActive(false);
                    RailCorner.SetActive(true);
                    break;
                default:
                    Debug.LogError("RailEntity: ConnectTiles: Invalid direction difference");
                    break;
            }
        }
    }
    private void OnDestroy()
    {
        if (OccupyingMineCart != null)
        {
            OccupyingMineCart.ExplodeWithDestruction();
        }
    }
}