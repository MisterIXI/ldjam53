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
            gridTile.SetResourceInfo(SettingsManager.GridSettings.TileMaterials[0], ResourceType.Shroom);
            var neighbours = HexHelper.GetNeighboursOddQ(gridTile.TileIndex);
            foreach (var neighbour in neighbours)
            {
                HexGrid.GetTile(neighbour).SetResourceInfo(SettingsManager.GridSettings.TileMaterials[0], ResourceType.Shroom);
            }
        }
        if (GUILayout.Button("SetTileTypeWater"))
        {
            gridTile.SetResourceInfo(SettingsManager.GridSettings.TileMaterials[1], ResourceType.Water);
            var neighbours = HexHelper.GetNeighboursOddQ(gridTile.TileIndex);
            foreach (var neighbour in neighbours)
            {
                HexGrid.GetTile(neighbour).SetResourceInfo(SettingsManager.GridSettings.TileMaterials[1], ResourceType.Water);
            }
        }
        if (GUILayout.Button("SetTileTypeCrystal"))
        {
            gridTile.SetResourceInfo(SettingsManager.GridSettings.TileMaterials[2], ResourceType.Crystal);
            var neighbours = HexHelper.GetNeighboursOddQ(gridTile.TileIndex);
            foreach (var neighbour in neighbours)
            {
                HexGrid.GetTile(neighbour).SetResourceInfo(SettingsManager.GridSettings.TileMaterials[2], ResourceType.Crystal);
            }
        }
        if (GUILayout.Button("SetTileTypeHoney"))
        {
            gridTile.SetResourceInfo(SettingsManager.GridSettings.TileMaterials[3], ResourceType.Honey);
            var neighbours = HexHelper.GetNeighboursOddQ(gridTile.TileIndex);
            foreach (var neighbour in neighbours)
            {
                HexGrid.GetTile(neighbour).SetResourceInfo(SettingsManager.GridSettings.TileMaterials[3], ResourceType.Honey);
            }
        }
        // add label that shows the index of the tile
        EditorGUILayout.LabelField("Index: " + gridTile.TileIndex);

    }
}