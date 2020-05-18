using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : UnityEditor.Editor
    {
        #region Public Methods

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Collect planet tiles"))
            {
                Planet planet = target as Planet;
                planet.GetTiles();
                Debug.Log($"Tiles collected.");
            }
            
            if (GUILayout.Button("Collect next tiles position"))
            {
                Planet planet = target as Planet;
                planet.CollectNextTiles();
                Debug.Log($"Collected next tiles");
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Save tiles meshes into assets"))
            {
                Planet planet = target as Planet;
                planet.SaveMeshes();
                Debug.Log($"Saved int assets.");
            }
        }

        #endregion
    }
}