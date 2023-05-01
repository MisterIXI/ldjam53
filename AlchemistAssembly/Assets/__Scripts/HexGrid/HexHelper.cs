using System.Collections.Generic;
using UnityEngine;
public class HexHelper : MonoBehaviour
{
    public static Vector2Int CubeToOddQ(Vector3Int cube)
    {
        int col = cube.x;
        int row = cube.z + (cube.x - (cube.x & 1)) / 2;
        return new Vector2Int(col, row);
    }


    public static Vector3Int OddQToCube(Vector2Int oddQ)
    {
        int q = oddQ.x;
        int r = oddQ.y - (oddQ.x - (oddQ.x & 1)) / 2;
        int s = -q - r;
        return new Vector3Int(q, s, r);
    }


    public static Vector3Int PixelToFlatHex(Vector2 point)
    {
        float q = (2 / 3f * point.x) / HexGrid.HEX_SIZE;
        float r = (-1 / 3f * point.x + (Mathf.Sqrt(3) / 3f * point.y)) / HexGrid.HEX_SIZE;
        return CubeRound(new Vector3(q, -q - r, r));
    }

    public static Vector3Int CubeRound(Vector3 point)
    {
        float rq = Mathf.Round(point.x);
        float rr = Mathf.Round(point.y);
        float rs = Mathf.Round(point.z);

        float q_diff = Mathf.Abs(rq - point.x);
        float r_diff = Mathf.Abs(rr - point.y);
        float s_diff = Mathf.Abs(rs - point.z);

        if (q_diff > r_diff && q_diff > s_diff)
        {
            rq = -rr - rs;
        }
        else if (r_diff > s_diff)
        {
            rr = -rq - rs;
        }
        else
        {
            rs = -rq - rr;
        }
        return new Vector3Int(Mathf.RoundToInt(rq), Mathf.RoundToInt(rr), Mathf.RoundToInt(rs));
    }

    public static List<Vector2Int> GetNeighboursOddQ(Vector2Int index, bool includeSelf = false)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        if (includeSelf)
            neighbours.Add(index);
        // add above and below
        if (index.y + 1 < HexGrid.GridSize.y)
            neighbours.Add(new Vector2Int(index.x, index.y + 1));
        if (index.y - 1 >= 0)
            neighbours.Add(new Vector2Int(index.x, index.y - 1));
        // check if even
        if ((index.x & 1) != 0)
        {
            // add left and right with positive offset
            if (index.x - 1 >= 0)
                neighbours.Add(new Vector2Int(index.x - 1, index.y));
            if (index.x - 1 >= 0 && index.y + 1 < HexGrid.GridSize.y)
                neighbours.Add(new Vector2Int(index.x - 1, index.y + 1));
            if (index.x + 1 < HexGrid.GridSize.x)
                neighbours.Add(new Vector2Int(index.x + 1, index.y));
            if (index.x + 1 < HexGrid.GridSize.x && index.y + 1 < HexGrid.GridSize.y)
                neighbours.Add(new Vector2Int(index.x + 1, index.y + 1));
        }
        else
        {
            // add left and right with negative offset
            if (index.x - 1 >= 0)
                neighbours.Add(new Vector2Int(index.x - 1, index.y));
            if (index.x - 1 >= 0 && index.y - 1 >= 0)
                neighbours.Add(new Vector2Int(index.x - 1, index.y - 1));
            if (index.x + 1 < HexGrid.GridSize.x)
                neighbours.Add(new Vector2Int(index.x + 1, index.y));
            if (index.x + 1 < HexGrid.GridSize.x && index.y - 1 >= 0)
                neighbours.Add(new Vector2Int(index.x + 1, index.y - 1));
        }
        return neighbours;
    }

    public static HexDirection GetDirection(Vector2Int start, Vector2Int end)
    {
        Vector2Int diff = end - start;
        if (diff.x == 0 && diff.y == 1)
            return HexDirection.Up;
        if (diff.x == 0 && diff.y == -1)
            return HexDirection.Down;
        bool isEven = (start.x & 1) == 0;
        if (isEven)
        {
            if (diff.x == 1 && diff.y == 0)
                return HexDirection.RightUp;
            if (diff.x == 1 && diff.y == -1)
                return HexDirection.RightDown;
            if (diff.x == -1 && diff.y == 0)
                return HexDirection.LeftUp;
            if (diff.x == -1 && diff.y == -1)
                return HexDirection.LeftDown;
        }
        else
        {
            if (diff.x == 1 && diff.y == 1)
                return HexDirection.RightUp;
            if (diff.x == 1 && diff.y == 0)
                return HexDirection.RightDown;
            if (diff.x == -1 && diff.y == 1)
                return HexDirection.LeftUp;
            if (diff.x == -1 && diff.y == 0)
                return HexDirection.LeftDown;
        }
        Debug.LogError($"Invalid direction from {start} to {end} with diff {diff}");
        throw new System.Exception("Invalid direction");
    }

    public static Vector2Int StepInDirection(Vector2Int index, HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.Up:
                return new Vector2Int(index.x, index.y + 1);
            case HexDirection.Down:
                return new Vector2Int(index.x, index.y - 1);
            default:
                if ((index.x & 1) == 0)
                {
                    // even column
                    switch (direction)
                    {
                        case HexDirection.RightUp:
                            return new Vector2Int(index.x + 1, index.y);
                        case HexDirection.RightDown:
                            return new Vector2Int(index.x + 1, index.y - 1);
                        case HexDirection.LeftUp:
                            return new Vector2Int(index.x - 1, index.y);
                        case HexDirection.LeftDown:
                            return new Vector2Int(index.x - 1, index.y - 1);
                        default:
                            throw new System.Exception("Invalid direction");
                    }
                }
                else
                {
                    // odd column
                    switch (direction)
                    {
                        case HexDirection.RightUp:
                            return new Vector2Int(index.x + 1, index.y + 1);
                        case HexDirection.RightDown:
                            return new Vector2Int(index.x + 1, index.y);
                        case HexDirection.LeftUp:
                            return new Vector2Int(index.x - 1, index.y + 1);
                        case HexDirection.LeftDown:
                            return new Vector2Int(index.x - 1, index.y);
                        default:
                            throw new System.Exception("Invalid direction");
                    }
                }
        }
    }
}

// don't change the order of these
public enum HexDirection
{
    Up,
    RightUp,
    RightDown,
    Down,
    LeftDown,
    LeftUp
}