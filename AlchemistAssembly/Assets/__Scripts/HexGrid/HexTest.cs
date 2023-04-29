using System.Linq;
using UnityEngine;

public class HexTest : MonoBehaviour
{
    private void FixedUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        // only while in playmode
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(HexGrid.GetTilePosition(HexGrid.GetTileIndex(transform.position)), 0.2f);
        }
    }

    private static Vector2Int? startPoint;
    public static void FindStreetPath(Vector2Int pos)
    {
        if (startPoint == null)
        {
            startPoint = pos;
            Debug.Log("Start point set to " + startPoint);
            HexGrid.GetTile(pos).GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            var path = HexGrid.GetPathToPos(startPoint.Value, pos);
            Debug.Log("Path from " + startPoint + " to " + pos + " is " + path.Count + " tiles long.");
            foreach (var tile in path)
            {
                tile.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
            path.First().GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            path.Last().GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
            startPoint = null;
        }
    }
}