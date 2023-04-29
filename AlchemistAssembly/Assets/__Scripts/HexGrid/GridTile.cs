using UnityEngine;

public class GridTile : MonoBehaviour
{

    public Vector2Int TileIndex { get; private set; }
    public bool IsInitialized => TileIndex != null;
    public void Initialize(Vector2Int tileIndex)
    {
        TileIndex = tileIndex;
    }
}