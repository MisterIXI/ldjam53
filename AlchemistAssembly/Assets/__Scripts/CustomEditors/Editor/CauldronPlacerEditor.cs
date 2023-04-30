using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CauldronPlacer))]
public class CauldronPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Activate"))
        {
            ((CauldronPlacer)target).Activate();
        }
        if(GUILayout.Button("Deactivate"))
        {
            ((CauldronPlacer)target).Deactivate();
        }
    }
}