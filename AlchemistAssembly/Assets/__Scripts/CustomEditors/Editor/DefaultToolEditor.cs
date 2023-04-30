using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultTool))]
public class DefaultToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Activate"))
        {
            ((DefaultTool)target).Activate();
        }
        if (GUILayout.Button("Deactivate"))
        {
            ((DefaultTool)target).Deactivate();
        }
    }
}