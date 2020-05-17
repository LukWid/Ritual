using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private List<Tile> planetTiles;
    [SerializeField]
    private Color editorPlanetColor;
    [SerializeField]
    private int iterationLevel;
    
    public List<Tile> PlanetTiles => planetTiles;
    public Transform planetTransform => transform;
    public Color EditorPlanetColor { get => editorPlanetColor;}

    public void GetTiles()
    {
        planetTiles = GetComponentsInChildren<Tile>().ToList();
        planetTiles.ForEach(tile => tile.Planet = this);
    }

    public void SaveMeshes()
    {
        if (planetTiles == null)
        {
            GetTiles();
        }
        
        for (int index = 0; index < planetTiles.Count; index++)
        {
            var tile = planetTiles[index];
            Mesh mesh = new Mesh();
            mesh = tile.GetComponent<MeshFilter>().sharedMesh;
            AssetDatabase.CreateAsset(mesh, $"Assets/Planet/Meshes/Iteration{iterationLevel}/Tile{index}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
