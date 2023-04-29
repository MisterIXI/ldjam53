using UnityEngine;

[SelectionBase]
public class GridTile : MonoBehaviour
{

    public Vector2Int TileIndex { get; private set; }
    public bool IsInitialized => TileIndex != null;
    public void Initialize(Vector2Int tileIndex)
    {
        TileIndex = tileIndex;
    }

    public void PaintNeighbours()
    {
        var neighbours = HexHelper.GetNeighboursOddQ(TileIndex);
        foreach (var neighbour in neighbours)
        {
            GridTile tile = HexGrid.GetTile(neighbour);
            if (tile != null)
            {
                tile.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}