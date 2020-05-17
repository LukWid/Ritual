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
            }
            
            if (GUILayout.Button("Save tiles meshes into assets"))
            {
                Planet planet = target as Planet;
                planet.SaveMeshes();
            }
        }

        #endregion
    }
}