using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                newTile.gameObject.SetActive(false);
            }
        }
        FillResources();
        StartAnimation();
    }
    public void StartAnimation()
    {
        StartCoroutine(AnimateGrid());
    }

    private IEnumerator AnimateGrid()
    {

        for (int y = 0; y < _gridSettings.GridSize.y; y++)
        {
            for (int x = 0; x < _gridSettings.GridSize.x; x++)
            {
                GridTile tile = _gridTiles[x, y];
                tile.gameObject.SetActive(true);
                tile.StartAnimation();
                // yield return new WaitForSeconds(0.01f);
            }
            yield return null;
            yield return null;
            yield return null;
        }
        PlaceableManager.PlaceObject(_gridSettings.WitchHousePrefab, _gridTiles[24, 34]);
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
    private void FillResources()
    {
        var shrooms = new Vector2Int[] { new Vector2Int(24, 13), new Vector2Int(24, 14), new Vector2Int(25, 12), new Vector2Int(25, 13), new Vector2Int(25, 14), new Vector2Int(26, 13), new Vector2Int(26, 14), new Vector2Int(31, 5), new Vector2Int(31, 6), new Vector2Int(32, 5), new Vector2Int(32, 6), new Vector2Int(32, 7), new Vector2Int(33, 5), new Vector2Int(33, 6), new Vector2Int(37, 19), new Vector2Int(37, 20), new Vector2Int(38, 12), new Vector2Int(38, 13), new Vector2Int(38, 19), new Vector2Int(38, 20), new Vector2Int(38, 21), new Vector2Int(39, 11), new Vector2Int(39, 12), new Vector2Int(39, 13), new Vector2Int(39, 19), new Vector2Int(39, 20), new Vector2Int(40, 6), new Vector2Int(40, 7), new Vector2Int(40, 12), new Vector2Int(40, 13), new Vector2Int(41, 5), new Vector2Int(41, 6), new Vector2Int(41, 7), new Vector2Int(42, 6), new Vector2Int(42, 7), new Vector2Int(45, 18), new Vector2Int(45, 19), new Vector2Int(46, 3), new Vector2Int(46, 4), new Vector2Int(46, 18), new Vector2Int(46, 19), new Vector2Int(46, 20), new Vector2Int(46, 24), new Vector2Int(46, 25), new Vector2Int(47, 2), new Vector2Int(47, 3), new Vector2Int(47, 4), new Vector2Int(47, 18), new Vector2Int(47, 19), new Vector2Int(47, 23), new Vector2Int(47, 24), new Vector2Int(47, 25), new Vector2Int(48, 3), new Vector2Int(48, 4), new Vector2Int(48, 24), new Vector2Int(48, 25) };
        for (int i = 0; i < shrooms.Length; i++)
        {
            _gridTiles[shrooms[i].x, shrooms[i].y].SetResourceInfo(_gridSettings.TileColors[0], ResourceType.Shroom);
        }
        var water = new Vector2Int[] { new Vector2Int(2, 14), new Vector2Int(2, 15), new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 13), new Vector2Int(3, 14), new Vector2Int(3, 15), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(4, 5), new Vector2Int(4, 14), new Vector2Int(4, 15), new Vector2Int(5, 3), new Vector2Int(5, 4), new Vector2Int(7, 8), new Vector2Int(7, 9), new Vector2Int(8, 8), new Vector2Int(8, 9), new Vector2Int(8, 10), new Vector2Int(9, 8), new Vector2Int(9, 9), new Vector2Int(13, 17), new Vector2Int(13, 18), new Vector2Int(14, 3), new Vector2Int(14, 4), new Vector2Int(14, 17), new Vector2Int(14, 18), new Vector2Int(14, 19), new Vector2Int(15, 2), new Vector2Int(15, 3), new Vector2Int(15, 4), new Vector2Int(15, 17), new Vector2Int(15, 18), new Vector2Int(16, 3), new Vector2Int(16, 4), new Vector2Int(17, 6), new Vector2Int(17, 7), new Vector2Int(18, 6), new Vector2Int(18, 7), new Vector2Int(18, 8), new Vector2Int(18, 20), new Vector2Int(18, 21), new Vector2Int(19, 6), new Vector2Int(19, 7), new Vector2Int(19, 19), new Vector2Int(19, 20), new Vector2Int(19, 21), new Vector2Int(20, 20), new Vector2Int(20, 21), new Vector2Int(23, 26), new Vector2Int(23, 27), new Vector2Int(24, 26), new Vector2Int(24, 27), new Vector2Int(24, 28), new Vector2Int(25, 26), new Vector2Int(25, 27) };
        for (int i = 0; i < water.Length; i++)
        {
            _gridTiles[water[i].x, water[i].y].SetResourceInfo(_gridSettings.TileColors[1], ResourceType.Water);
        }
        var crystals = new Vector2Int[] { new Vector2Int(4, 44), new Vector2Int(4, 45), new Vector2Int(5, 43), new Vector2Int(5, 44), new Vector2Int(5, 45), new Vector2Int(6, 44), new Vector2Int(6, 45), new Vector2Int(12, 46), new Vector2Int(12, 47), new Vector2Int(13, 45), new Vector2Int(13, 46), new Vector2Int(13, 47), new Vector2Int(14, 46), new Vector2Int(14, 47), new Vector2Int(35, 37), new Vector2Int(35, 38), new Vector2Int(36, 37), new Vector2Int(36, 38), new Vector2Int(36, 39), new Vector2Int(37, 37), new Vector2Int(37, 38), new Vector2Int(39, 44), new Vector2Int(39, 45), new Vector2Int(40, 44), new Vector2Int(40, 45), new Vector2Int(40, 46), new Vector2Int(41, 44), new Vector2Int(41, 45), new Vector2Int(42, 39), new Vector2Int(42, 40), new Vector2Int(43, 38), new Vector2Int(43, 39), new Vector2Int(43, 40), new Vector2Int(44, 39), new Vector2Int(44, 40), new Vector2Int(44, 44), new Vector2Int(44, 45), new Vector2Int(45, 43), new Vector2Int(45, 44), new Vector2Int(45, 45), new Vector2Int(46, 44), new Vector2Int(46, 45) };
        for (int i = 0; i < crystals.Length; i++)
        {
            _gridTiles[crystals[i].x, crystals[i].y].SetResourceInfo(_gridSettings.TileColors[2], ResourceType.Crystal);
        }
        var honey = new Vector2Int[] { new Vector2Int(4, 24), new Vector2Int(4, 25), new Vector2Int(5, 23), new Vector2Int(5, 24), new Vector2Int(5, 25), new Vector2Int(5, 30), new Vector2Int(5, 31), new Vector2Int(6, 24), new Vector2Int(6, 25), new Vector2Int(6, 30), new Vector2Int(6, 31), new Vector2Int(6, 32), new Vector2Int(7, 30), new Vector2Int(7, 31), new Vector2Int(10, 24), new Vector2Int(10, 25), new Vector2Int(11, 23), new Vector2Int(11, 24), new Vector2Int(11, 25), new Vector2Int(12, 24), new Vector2Int(12, 25), new Vector2Int(13, 36), new Vector2Int(13, 37), new Vector2Int(14, 29), new Vector2Int(14, 30), new Vector2Int(14, 36), new Vector2Int(14, 37), new Vector2Int(14, 38), new Vector2Int(15, 28), new Vector2Int(15, 29), new Vector2Int(15, 30), new Vector2Int(15, 36), new Vector2Int(15, 37), new Vector2Int(16, 29), new Vector2Int(16, 30), new Vector2Int(21, 45), new Vector2Int(21, 46), new Vector2Int(22, 45), new Vector2Int(22, 46), new Vector2Int(22, 47), new Vector2Int(23, 45), new Vector2Int(23, 46), new Vector2Int(30, 45), new Vector2Int(30, 46), new Vector2Int(31, 44), new Vector2Int(31, 45), new Vector2Int(31, 46), new Vector2Int(32, 45), new Vector2Int(32, 46) };
    }
    public void PrintResources()
    {
        string shrooms = "";
        string water = "";
        string crystals = "";
        string honey = "";
        // loop through all tiles
        for (int x = 0; x < _gridTiles.GetLength(0); x++)
        {
            for (int y = 0; y < _gridTiles.GetLength(1); y++)
            {
                // get the tile at the current index
                GridTile tile = _gridTiles[x, y];
                // if the tile is not null
                if (tile.ResourceType != ResourceType.Empty)
                {
                    switch (tile.ResourceType)
                    {
                        case ResourceType.Shroom:
                            shrooms += $"new Vector2Int({x}, {y}),";
                            break;
                        case ResourceType.Water:
                            water += $"new Vector2Int({x}, {y}),";
                            break;
                        case ResourceType.Crystal:
                            crystals += $"new Vector2Int({x}, {y}),";
                            break;
                        case ResourceType.Honey:
                            honey += $"new Vector2Int({x}, {y}),";
                            break;
                    }
                }
            }
        }
        Debug.Log("shrooms: " + shrooms);
        Debug.Log("water: " + water);
        Debug.Log("crystals: " + crystals);
        Debug.Log("honey: " + honey);
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