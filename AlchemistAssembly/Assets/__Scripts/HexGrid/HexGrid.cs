using System.Collections.Generic;
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
    public static Vector2Int GridSize => Instance._gridSettings.GridSize;
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
            Debug.LogWarning("Position is out of bounds");
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

    public static bool IsValidIndex(Vector2Int index)
    {
        return index.x >= 0 && index.x < Instance._gridTiles.GetLength(0) && index.y >= 0 && index.y < Instance._gridTiles.GetLength(1);
    }

    public static List<GridTile> GetPathToPos(Vector2Int start, Vector2Int target)
    {
        List<GridTile> path = new List<GridTile>();
        if (!IsValidIndex(start) || !IsValidIndex(target))
        {
            Debug.LogError("Invalid start or target index");
            return path;
        }
        if (start == target)
        {
            path.Add(GetTile(start));
            return path;
        }
        Vector2Int current = start;
        Vector2Int startAxis = HexHelper.OddQToCube(start).ToAxisSystem();
        Vector2Int targetAxis = HexHelper.OddQToCube(target).ToAxisSystem();
        int rSteps = targetAxis.x - startAxis.x;
        int qSteps = targetAxis.y - startAxis.y;
        List<Vector2Int> axisPath = new List<Vector2Int>();
        while (rSteps != 0 && qSteps != 0 && Mathf.Sign(rSteps) != Mathf.Sign(qSteps))
        {
            axisPath.Add(new Vector2Int((int)Mathf.Sign(rSteps), (int)Mathf.Sign(qSteps)));
            rSteps -= (int)Mathf.Sign(rSteps);
            qSteps -= (int)Mathf.Sign(qSteps);
        }
        while (rSteps != 0)
        {
            axisPath.Add(new Vector2Int((int)Mathf.Sign(rSteps), 0));
            rSteps -= (int)Mathf.Sign(rSteps);
        }
        while (qSteps != 0)
        {
            axisPath.Add(new Vector2Int(0, (int)Mathf.Sign(qSteps)));
            qSteps -= (int)Mathf.Sign(qSteps);
        }
        Vector2Int currentPoint = startAxis;
        path.Add(GetTile(HexHelper.CubeToOddQ(currentPoint.ToCubeSystem())));
        foreach (Vector2Int step in axisPath)
        {
            currentPoint += step;
            Vector2Int oddQ = HexHelper.CubeToOddQ((currentPoint).ToCubeSystem());
            if (!IsValidIndex(oddQ))
            {
                path.Clear();
                return path;
            }
            path.Add(GetTile(oddQ));
        }
        /*
        description:
        first convert to axis coordinates
        first get all points along X axis to until source.x == target.x
        then get all points along Y axis until source.y == target.y
        now calculate how many X are in Y (X-Y) and see how much is remaining
        e.g. (5,3) in steps, then 3 steps in third axis, and then the remaining (2,0)
        e.g. (2,5) in steps, then 2 steps in third axis, and then the remaining (0,3)
        e.g. (3,3) in steps, then 3 steps in third axis, and then the remaining (0,0)
        e.g. (0,3) in steps, then 0 steps in third axis, and then the remaining (0,3)
        e.g. (-3,4) in steps, then 

        pseudocode to find shortest path:

        */
        return path;
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