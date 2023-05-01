using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BitchHouse))]
public class BitchHouseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BitchHouse bitchHouse = (BitchHouse)target;
        GUILayout.Label("Red Potions: " + bitchHouse.redPotCounter);
        GUILayout.Label("Blue Potions: " + bitchHouse.bluePotCounter);
        GUILayout.Label("Yellow Potions: " + bitchHouse.yellowPotCounter);
        GUILayout.Label("Yin Yang Potions: " + bitchHouse.purplePotCounter);
    }
}