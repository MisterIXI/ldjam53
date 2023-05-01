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
        if (GUILayout.Button("SetTileTypeShroom"))
        {
            gridTile.SetResourceInfo(SettingsManager.GridSettings.TileColors[0], ResourceType.Shroom);
            var neighbours = HexHelper.GetNeighboursOddQ(gridTile.TileIndex);
            foreach (var neighbour in neighbours)
            {
                HexGrid.GetTile(neighbour).SetResourceInfo(SettingsManager.GridSettings.TileColors[0], ResourceType.Shroom);
            }
        }
        // add label that shows the index of the tile
        EditorGUILayout.LabelField("Index: " + gridTile.TileIndex);

    }
}