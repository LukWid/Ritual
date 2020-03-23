using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hexsphere))]
public class HexsphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save into assets"))
        {
            Hexsphere hexsphere = target as Hexsphere;
            hexsphere.SaveMeshes();
        }
    }
}
