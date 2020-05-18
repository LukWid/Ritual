using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetSystem))]
public class PlanterSystemEditor : UnityEditor.Editor
{
    public void OnSceneGUI()
    {
        Handles.BeginGUI();
        PlanetSystem system = target as PlanetSystem;

        if (GUILayout.Button("Iteration 1", GUILayout.Width(100)))
        {
            system.ShowIteration(0);
        }
        if (GUILayout.Button("Iteration 2", GUILayout.Width(100)))
        {
            system.ShowIteration(1);
        }
        if (GUILayout.Button("Iteration 3", GUILayout.Width(100)))
        {
            system.ShowIteration(2);
        }
        if (GUILayout.Button("Iteration 4", GUILayout.Width(100)))
        {
            system.ShowIteration(3);
        }

        Handles.EndGUI();
    }
}
