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
}