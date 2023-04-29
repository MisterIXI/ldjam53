using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public const float HEX_SIZE = 1f;
    public const float OUTER_HEX_SIZE = 2f;
    public const float INNER_HEX_SIZE = 1.73205080757f;
    private Vector3 Dir_UL = new Vector3(-OUTER_HEX_SIZE * 0.25f, 0, INNER_HEX_SIZE / 2f);
    private Vector3 Dir_UR = new Vector3(OUTER_HEX_SIZE * 0.25f, 0, INNER_HEX_SIZE / 2f);
    private Vector3 Dir_R = new Vector3(OUTER_HEX_SIZE * 0.5f, 0, 0);
    private Vector3 Dir_DR = new Vector3(OUTER_HEX_SIZE * 0.25f, 0, -INNER_HEX_SIZE / 2f);
    private Vector3 Dir_DL = new Vector3(-OUTER_HEX_SIZE * 0.25f, 0, -INNER_HEX_SIZE / 2f);
    private Vector3 Dir_L = new Vector3(-OUTER_HEX_SIZE * 0.5f, 0, 0);
    // Hexagonal grid with an flat-top odd-q layout. Every second column is offset by half a hexagon.

    public static HexGrid Instance { get; private set; }
    private GridTile[,] _gridTiles;
    [field: SerializeField] private bool _drawGizmos = false;
    [field: SerializeField] private GridSettings _gridSettings = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        _gridTiles = new GridTile[_gridSettings.GridSize.x, _gridSettings.GridSize.y];
        Vector3 currentPos = Vector3.zero;

        for (int x = 0; x < _gridSettings.GridSize.x; x++)
        {
            for (int y = 0; y < _gridSettings.GridSize.y; y++)
            {
                currentPos = GetTilePosition(new Vector2Int(x, y));
                // create a new tile
                GridTile newTile = Instantiate(_gridSettings.BaseTilePrefab, currentPos, Quaternion.identity, transform);
                newTile.Initialize(new Vector2Int(x, y));
                _gridTiles[x, y] = newTile;
            }
        }
    }
    public static GridTile GetTile(Vector3 position)
    {
        return GetTile(GetTileIndex(position));
    }

    public static GridTile GetTile(Vector2Int position)
    {
        if (position.x < 0 || position.x >= Instance._gridTiles.GetLength(0) || position.y < 0 || position.y >= Instance._gridTiles.GetLength(1))
        {
            Debug.LogError("Position is out of bounds");
            return null;
        }
        return Instance._gridTiles[position.x, position.y];
    }

    public static Vector2Int GetTileIndex(Vector3 position)
    {
        Vector3Int cubeCoord = HexHelper.PixelToFlatHex(new Vector2(position.x, position.z));
        Vector2Int index = HexHelper.CubeToOddQ(cubeCoord);
        return index;
    }

    public static Vector3 GetTilePosition(Vector2Int index)
    {
        Vector3 position = new Vector3();
        // calculate the position of the tile at the given index
        position.x = index.x * OUTER_HEX_SIZE * 0.75f;
        position.z = index.y * INNER_HEX_SIZE;
        if (index.x % 2 == 1)
            position.z += INNER_HEX_SIZE / 2f;
        return position;
    }

    [field: SerializeField] private Vector2Int _gizmosSize;
    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Vector3 currentPos = transform.position;

            for (int x = 0; x < _gizmosSize.x; x++)
            {
                for (int y = 0; y < _gizmosSize.y; y++)
                {
                    currentPos = GetTilePosition(new Vector2Int(x, y));
                    // draw hexagon lines
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(currentPos + Dir_UL, currentPos + Dir_UR);
                    Gizmos.DrawLine(currentPos + Dir_UR, currentPos + Dir_R);
                    Gizmos.DrawLine(currentPos + Dir_R, currentPos + Dir_DR);
                    Gizmos.DrawLine(currentPos + Dir_DR, currentPos + Dir_DL);
                    Gizmos.DrawLine(currentPos + Dir_DL, currentPos + Dir_L);
                    Gizmos.DrawLine(currentPos + Dir_L, currentPos + Dir_UL);
                }
            }
        }
    }
}