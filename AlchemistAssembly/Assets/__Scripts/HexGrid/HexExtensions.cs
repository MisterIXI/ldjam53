using UnityEngine;

public static class HexExtensions
{
    public static Vector2Int ToAxisSystem(this Vector3Int cube)
    {
        return new Vector2Int(cube.x, cube.z);
    }

    public static Vector3Int ToCubeSystem(this Vector2Int axis)
    {
        return new Vector3Int(axis.x, -axis.x - axis.y, axis.y);
    }

}