using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridTile))]
public class GridTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridTile gridTile = (GridTile)target;
        if (GUILayout.Button("PaintNeighbours"))
        {
            gridTile.PaintNeighbours();
        }
        if (GUILayout.Button("TriggerPath"))
        {
            gridTile.TriggerPath();
        }
        // add label that shows the index of the tile
        EditorGUILayout.LabelField("Index: " + gridTile.TileIndex);

    }
}