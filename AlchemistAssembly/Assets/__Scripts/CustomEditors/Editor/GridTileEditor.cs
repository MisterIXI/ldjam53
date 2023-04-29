using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridTile))]
public class GridTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("PaintNeighbours"))
        {
            GridTile gridTile = (GridTile)target;
            gridTile.PaintNeighbours();
        }

    }
}