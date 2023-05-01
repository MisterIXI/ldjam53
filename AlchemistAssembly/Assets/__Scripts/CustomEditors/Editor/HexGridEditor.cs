using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HexGrid hexGrid = (HexGrid)target;
        if (GUILayout.Button("PrintResources"))
        {
            hexGrid.PrintResources();
        }
    }
}