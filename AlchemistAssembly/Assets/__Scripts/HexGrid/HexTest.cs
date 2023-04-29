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
}