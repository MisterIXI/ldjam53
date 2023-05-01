using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactoryBuilding))]
public class FactoryBuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Send Minecart"))
            ((FactoryBuilding)target).SendOutput();
    }
}