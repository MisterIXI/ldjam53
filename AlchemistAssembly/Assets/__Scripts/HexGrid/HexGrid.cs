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
        var shrooms = new Vector2Int[] { new Vector2Int(8, 32), new Vector2Int(8, 33), new Vector2Int(9, 29), new Vector2Int(9, 30), new Vector2Int(9, 31), new Vector2Int(9, 32), new Vector2Int(9, 33), new Vector2Int(10, 29), new Vector2Int(10, 30), new Vector2Int(10, 31), new Vector2Int(10, 32), new Vector2Int(10, 33), new Vector2Int(11, 29), new Vector2Int(11, 30), new Vector2Int(11, 31), new Vector2Int(11, 32), new Vector2Int(11, 33), new Vector2Int(12, 28), new Vector2Int(12, 29), new Vector2Int(12, 30), new Vector2Int(12, 31), new Vector2Int(12, 32), new Vector2Int(12, 33), new Vector2Int(12, 34), new Vector2Int(13, 27), new Vector2Int(13, 28), new Vector2Int(13, 29), new Vector2Int(13, 30), new Vector2Int(13, 31), new Vector2Int(13, 32), new Vector2Int(13, 33), new Vector2Int(14, 28), new Vector2Int(14, 29), new Vector2Int(14, 30), new Vector2Int(14, 31), new Vector2Int(14, 32), new Vector2Int(14, 33), new Vector2Int(14, 34), new Vector2Int(15, 30), new Vector2Int(15, 31), new Vector2Int(15, 32), new Vector2Int(15, 33), new Vector2Int(15, 34), new Vector2Int(16, 33), new Vector2Int(16, 34), new Vector2Int(24, 13), new Vector2Int(24, 14), new Vector2Int(25, 5), new Vector2Int(25, 6), new Vector2Int(25, 10), new Vector2Int(25, 11), new Vector2Int(25, 12), new Vector2Int(25, 13), new Vector2Int(25, 14), new Vector2Int(26, 5), new Vector2Int(26, 6), new Vector2Int(26, 7), new Vector2Int(26, 8), new Vector2Int(26, 9), new Vector2Int(26, 10), new Vector2Int(26, 11), new Vector2Int(26, 12), new Vector2Int(26, 13), new Vector2Int(26, 14), new Vector2Int(27, 5), new Vector2Int(27, 6), new Vector2Int(27, 7), new Vector2Int(27, 8), new Vector2Int(27, 9), new Vector2Int(27, 10), new Vector2Int(27, 11), new Vector2Int(27, 12), new Vector2Int(27, 13), new Vector2Int(28, 5), new Vector2Int(28, 6), new Vector2Int(28, 7), new Vector2Int(28, 8), new Vector2Int(28, 9), new Vector2Int(28, 10), new Vector2Int(28, 11), new Vector2Int(28, 12), new Vector2Int(28, 13), new Vector2Int(28, 14), new Vector2Int(29, 4), new Vector2Int(29, 5), new Vector2Int(29, 6), new Vector2Int(29, 7), new Vector2Int(29, 8), new Vector2Int(29, 9), new Vector2Int(29, 10), new Vector2Int(29, 11), new Vector2Int(29, 12), new Vector2Int(29, 13), new Vector2Int(30, 5), new Vector2Int(30, 6), new Vector2Int(30, 7), new Vector2Int(30, 8), new Vector2Int(30, 9), new Vector2Int(30, 10), new Vector2Int(30, 11), new Vector2Int(31, 5), new Vector2Int(31, 6), new Vector2Int(31, 7), new Vector2Int(31, 8), new Vector2Int(31, 9), new Vector2Int(31, 10), new Vector2Int(32, 5), new Vector2Int(32, 6), new Vector2Int(32, 7), new Vector2Int(32, 9), new Vector2Int(32, 10), new Vector2Int(32, 11), new Vector2Int(33, 5), new Vector2Int(33, 6), new Vector2Int(33, 9), new Vector2Int(33, 10), new Vector2Int(38, 12), new Vector2Int(38, 13), new Vector2Int(39, 11), new Vector2Int(39, 12), new Vector2Int(39, 13), new Vector2Int(40, 12), new Vector2Int(40, 13) };
        for (int i = 0; i < shrooms.Length; i++)
        {
            _gridTiles[shrooms[i].x, shrooms[i].y].SetResourceInfo(_gridSettings.TileMaterials[0], ResourceType.Shroom);
        }
        var water = new Vector2Int[] { new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 5), new Vector2Int(3, 6), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(4, 5), new Vector2Int(4, 6), new Vector2Int(4, 7), new Vector2Int(4, 8), new Vector2Int(4, 9), new Vector2Int(5, 3), new Vector2Int(5, 4), new Vector2Int(5, 5), new Vector2Int(5, 6), new Vector2Int(5, 7), new Vector2Int(5, 8), new Vector2Int(5, 9), new Vector2Int(6, 3), new Vector2Int(6, 4), new Vector2Int(6, 5), new Vector2Int(6, 6), new Vector2Int(6, 7), new Vector2Int(6, 8), new Vector2Int(6, 9), new Vector2Int(7, 3), new Vector2Int(7, 4), new Vector2Int(7, 5), new Vector2Int(7, 6), new Vector2Int(7, 7), new Vector2Int(7, 8), new Vector2Int(7, 9), new Vector2Int(8, 4), new Vector2Int(8, 5), new Vector2Int(8, 6), new Vector2Int(8, 7), new Vector2Int(8, 8), new Vector2Int(8, 9), new Vector2Int(8, 10), new Vector2Int(9, 3), new Vector2Int(9, 4), new Vector2Int(9, 5), new Vector2Int(9, 6), new Vector2Int(9, 7), new Vector2Int(9, 8), new Vector2Int(9, 9), new Vector2Int(10, 4), new Vector2Int(10, 5), new Vector2Int(10, 6), new Vector2Int(10, 7), new Vector2Int(10, 8), new Vector2Int(11, 6), new Vector2Int(11, 7), new Vector2Int(21, 25), new Vector2Int(21, 26), new Vector2Int(22, 25), new Vector2Int(22, 26), new Vector2Int(22, 27), new Vector2Int(23, 24), new Vector2Int(23, 25), new Vector2Int(23, 26), new Vector2Int(23, 27), new Vector2Int(24, 24), new Vector2Int(24, 25), new Vector2Int(24, 26), new Vector2Int(24, 27), new Vector2Int(24, 28), new Vector2Int(25, 24), new Vector2Int(25, 25), new Vector2Int(25, 26), new Vector2Int(25, 27), new Vector2Int(39, 3), new Vector2Int(39, 4), new Vector2Int(39, 44), new Vector2Int(39, 45), new Vector2Int(39, 46), new Vector2Int(39, 47), new Vector2Int(40, 3), new Vector2Int(40, 4), new Vector2Int(40, 5), new Vector2Int(40, 6), new Vector2Int(40, 7), new Vector2Int(40, 41), new Vector2Int(40, 42), new Vector2Int(40, 44), new Vector2Int(40, 45), new Vector2Int(40, 46), new Vector2Int(40, 47), new Vector2Int(40, 48), new Vector2Int(41, 3), new Vector2Int(41, 4), new Vector2Int(41, 5), new Vector2Int(41, 6), new Vector2Int(41, 7), new Vector2Int(41, 40), new Vector2Int(41, 41), new Vector2Int(41, 42), new Vector2Int(41, 43), new Vector2Int(41, 44), new Vector2Int(41, 45), new Vector2Int(41, 46), new Vector2Int(41, 47), new Vector2Int(42, 3), new Vector2Int(42, 4), new Vector2Int(42, 5), new Vector2Int(42, 6), new Vector2Int(42, 7), new Vector2Int(42, 39), new Vector2Int(42, 40), new Vector2Int(42, 41), new Vector2Int(42, 42), new Vector2Int(42, 43), new Vector2Int(42, 44), new Vector2Int(42, 45), new Vector2Int(42, 46), new Vector2Int(42, 47), new Vector2Int(43, 2), new Vector2Int(43, 3), new Vector2Int(43, 4), new Vector2Int(43, 5), new Vector2Int(43, 38), new Vector2Int(43, 39), new Vector2Int(43, 40), new Vector2Int(43, 41), new Vector2Int(43, 42), new Vector2Int(43, 43), new Vector2Int(43, 44), new Vector2Int(43, 45), new Vector2Int(43, 46), new Vector2Int(43, 47), new Vector2Int(44, 2), new Vector2Int(44, 3), new Vector2Int(44, 4), new Vector2Int(44, 5), new Vector2Int(44, 6), new Vector2Int(44, 39), new Vector2Int(44, 40), new Vector2Int(44, 41), new Vector2Int(44, 42), new Vector2Int(44, 43), new Vector2Int(44, 44), new Vector2Int(44, 45), new Vector2Int(44, 46), new Vector2Int(44, 47), new Vector2Int(45, 2), new Vector2Int(45, 3), new Vector2Int(45, 4), new Vector2Int(45, 5), new Vector2Int(45, 41), new Vector2Int(45, 42), new Vector2Int(45, 43), new Vector2Int(45, 44), new Vector2Int(45, 45), new Vector2Int(45, 46), new Vector2Int(45, 47), new Vector2Int(46, 3), new Vector2Int(46, 4), new Vector2Int(46, 44), new Vector2Int(46, 45), new Vector2Int(46, 46), new Vector2Int(46, 47), new Vector2Int(46, 48), new Vector2Int(47, 2), new Vector2Int(47, 3), new Vector2Int(47, 4), new Vector2Int(47, 46), new Vector2Int(47, 47), new Vector2Int(48, 3), new Vector2Int(48, 4) };
        for (int i = 0; i < water.Length; i++)
        {
            _gridTiles[water[i].x, water[i].y].SetResourceInfo(_gridSettings.TileMaterials[1], ResourceType.Water);
        }
        var crystals = new Vector2Int[] { new Vector2Int(0, 43), new Vector2Int(0, 44), new Vector2Int(0, 45), new Vector2Int(0, 46), new Vector2Int(0, 47), new Vector2Int(0, 48), new Vector2Int(0, 49), new Vector2Int(1, 43), new Vector2Int(1, 44), new Vector2Int(1, 45), new Vector2Int(1, 46), new Vector2Int(1, 47), new Vector2Int(1, 48), new Vector2Int(1, 49), new Vector2Int(2, 43), new Vector2Int(2, 44), new Vector2Int(2, 45), new Vector2Int(2, 46), new Vector2Int(2, 47), new Vector2Int(2, 48), new Vector2Int(2, 49), new Vector2Int(3, 43), new Vector2Int(3, 44), new Vector2Int(3, 45), new Vector2Int(3, 46), new Vector2Int(3, 47), new Vector2Int(3, 48), new Vector2Int(3, 49), new Vector2Int(4, 44), new Vector2Int(4, 45), new Vector2Int(4, 46), new Vector2Int(4, 47), new Vector2Int(4, 48), new Vector2Int(4, 49), new Vector2Int(5, 43), new Vector2Int(5, 44), new Vector2Int(5, 45), new Vector2Int(5, 46), new Vector2Int(5, 47), new Vector2Int(5, 48), new Vector2Int(5, 49), new Vector2Int(6, 44), new Vector2Int(6, 45), new Vector2Int(6, 46), new Vector2Int(6, 47), new Vector2Int(6, 48), new Vector2Int(6, 49), new Vector2Int(7, 46), new Vector2Int(7, 47), new Vector2Int(7, 48), new Vector2Int(7, 49), new Vector2Int(8, 48), new Vector2Int(8, 49), new Vector2Int(11, 48), new Vector2Int(11, 49), new Vector2Int(12, 46), new Vector2Int(12, 47), new Vector2Int(12, 48), new Vector2Int(12, 49), new Vector2Int(13, 17), new Vector2Int(13, 18), new Vector2Int(13, 45), new Vector2Int(13, 46), new Vector2Int(13, 47), new Vector2Int(13, 48), new Vector2Int(13, 49), new Vector2Int(14, 3), new Vector2Int(14, 4), new Vector2Int(14, 17), new Vector2Int(14, 18), new Vector2Int(14, 19), new Vector2Int(14, 46), new Vector2Int(14, 47), new Vector2Int(14, 48), new Vector2Int(14, 49), new Vector2Int(15, 2), new Vector2Int(15, 3), new Vector2Int(15, 4), new Vector2Int(15, 17), new Vector2Int(15, 18), new Vector2Int(15, 19), new Vector2Int(15, 20), new Vector2Int(15, 47), new Vector2Int(15, 48), new Vector2Int(15, 49), new Vector2Int(16, 3), new Vector2Int(16, 4), new Vector2Int(16, 5), new Vector2Int(16, 18), new Vector2Int(16, 19), new Vector2Int(16, 20), new Vector2Int(16, 21), new Vector2Int(16, 48), new Vector2Int(16, 49), new Vector2Int(17, 2), new Vector2Int(17, 3), new Vector2Int(17, 4), new Vector2Int(17, 5), new Vector2Int(17, 6), new Vector2Int(17, 7), new Vector2Int(17, 18), new Vector2Int(17, 19), new Vector2Int(17, 20), new Vector2Int(18, 2), new Vector2Int(18, 3), new Vector2Int(18, 4), new Vector2Int(18, 5), new Vector2Int(18, 6), new Vector2Int(18, 7), new Vector2Int(18, 8), new Vector2Int(18, 20), new Vector2Int(18, 21), new Vector2Int(19, 2), new Vector2Int(19, 3), new Vector2Int(19, 4), new Vector2Int(19, 5), new Vector2Int(19, 6), new Vector2Int(19, 7), new Vector2Int(19, 19), new Vector2Int(19, 20), new Vector2Int(19, 21), new Vector2Int(20, 4), new Vector2Int(20, 5), new Vector2Int(20, 6), new Vector2Int(20, 20), new Vector2Int(20, 21), new Vector2Int(21, 4), new Vector2Int(21, 5) };
        for (int i = 0; i < crystals.Length; i++)
        {
            _gridTiles[crystals[i].x, crystals[i].y].SetResourceInfo(_gridSettings.TileMaterials[2], ResourceType.Crystal);
        }
        var honey = new Vector2Int[] { new Vector2Int(2, 14), new Vector2Int(2, 15), new Vector2Int(3, 13), new Vector2Int(3, 14), new Vector2Int(3, 15), new Vector2Int(4, 14), new Vector2Int(4, 15), new Vector2Int(5, 13), new Vector2Int(5, 14), new Vector2Int(5, 15), new Vector2Int(5, 16), new Vector2Int(6, 13), new Vector2Int(6, 14), new Vector2Int(6, 15), new Vector2Int(6, 16), new Vector2Int(6, 17), new Vector2Int(7, 13), new Vector2Int(7, 14), new Vector2Int(7, 15), new Vector2Int(7, 16), new Vector2Int(8, 14), new Vector2Int(8, 15), new Vector2Int(8, 16), new Vector2Int(8, 17), new Vector2Int(9, 14), new Vector2Int(9, 15), new Vector2Int(9, 16), new Vector2Int(9, 17), new Vector2Int(10, 14), new Vector2Int(10, 15), new Vector2Int(10, 16), new Vector2Int(10, 17), new Vector2Int(11, 13), new Vector2Int(11, 14), new Vector2Int(11, 15), new Vector2Int(12, 14), new Vector2Int(12, 15), new Vector2Int(35, 37), new Vector2Int(35, 38), new Vector2Int(36, 37), new Vector2Int(36, 38), new Vector2Int(36, 39), new Vector2Int(37, 19), new Vector2Int(37, 20), new Vector2Int(37, 35), new Vector2Int(37, 36), new Vector2Int(37, 37), new Vector2Int(37, 38), new Vector2Int(38, 19), new Vector2Int(38, 20), new Vector2Int(38, 21), new Vector2Int(38, 35), new Vector2Int(38, 36), new Vector2Int(38, 37), new Vector2Int(38, 38), new Vector2Int(38, 39), new Vector2Int(39, 19), new Vector2Int(39, 20), new Vector2Int(39, 21), new Vector2Int(39, 22), new Vector2Int(39, 35), new Vector2Int(39, 36), new Vector2Int(39, 37), new Vector2Int(39, 38), new Vector2Int(39, 39), new Vector2Int(40, 19), new Vector2Int(40, 20), new Vector2Int(40, 21), new Vector2Int(40, 22), new Vector2Int(40, 23), new Vector2Int(40, 38), new Vector2Int(40, 39), new Vector2Int(41, 18), new Vector2Int(41, 19), new Vector2Int(41, 20), new Vector2Int(41, 21), new Vector2Int(41, 22), new Vector2Int(42, 18), new Vector2Int(42, 19), new Vector2Int(42, 20), new Vector2Int(42, 21), new Vector2Int(42, 22), new Vector2Int(42, 23), new Vector2Int(43, 17), new Vector2Int(43, 18), new Vector2Int(43, 19), new Vector2Int(43, 20), new Vector2Int(43, 21), new Vector2Int(43, 22), new Vector2Int(43, 23), new Vector2Int(43, 24), new Vector2Int(44, 18), new Vector2Int(44, 19), new Vector2Int(44, 20), new Vector2Int(44, 21), new Vector2Int(44, 22), new Vector2Int(44, 23), new Vector2Int(44, 24), new Vector2Int(44, 25), new Vector2Int(45, 18), new Vector2Int(45, 19), new Vector2Int(45, 20), new Vector2Int(45, 21), new Vector2Int(45, 22), new Vector2Int(45, 23), new Vector2Int(45, 24), new Vector2Int(46, 18), new Vector2Int(46, 19), new Vector2Int(46, 20), new Vector2Int(46, 21), new Vector2Int(46, 22), new Vector2Int(46, 23), new Vector2Int(46, 24), new Vector2Int(46, 25), new Vector2Int(47, 18), new Vector2Int(47, 19), new Vector2Int(47, 20), new Vector2Int(47, 21), new Vector2Int(47, 22), new Vector2Int(47, 23), new Vector2Int(47, 24), new Vector2Int(47, 25), new Vector2Int(48, 19), new Vector2Int(48, 20), new Vector2Int(48, 21), new Vector2Int(48, 24), new Vector2Int(48, 25), new Vector2Int(49, 19), new Vector2Int(49, 20) };
        for (int i = 0; i < honey.Length; i++)
        {
            _gridTiles[honey[i].x, honey[i].y].SetResourceInfo(_gridSettings.TileMaterials[3], ResourceType.Honey);
        }
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