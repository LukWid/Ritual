using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hexsphere))]
public class ChangeChildrenMaterial : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ChangeColor"))
        {
            Hexsphere targetObject = (Hexsphere)target;
            //targetObject.GetComponent<Hexsphere>().ChangeChildrenColor();
        }
    }
}
