using System.Collections.Generic;
using UnityEngine;

public class PlanetSystem : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private List<Hexsphere> planets;

    [SerializeField]
    private int currentIteration;

    #endregion

    #region Private Fields

    private List<Tile> currentTiles;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        for (int i = 0; i < planets.Count-1; i++)
        {
            GameObject currentPlanet = planets[i].gameObject;
            Debug.Log($"{currentPlanet}");
            CollectPlanetTiles(currentPlanet);
        }
    }

    private void CollectPlanetTiles(GameObject currentPlanet)
    {
        var children = currentPlanet.GetComponentsInChildren<Tile>();
        foreach (var child in children)
        {
            GetNextTile(child, currentPlanet.transform);
        }
    }

    #endregion

    #region Private Methods

    private void GetNextTile(Tile tile, Transform planet)
    {
        RaycastHit hit;
        Mesh childMesh = tile.gameObject.GetComponent<MeshFilter>().mesh;
        Vector3 meshMiddle = CalculateMeshMiddle(childMesh);
        Vector3 direction = ( meshMiddle - transform.position ).normalized;

        //SpawnCube(planet.transform.TransformPoint(meshMiddle), direction);
        
        meshMiddle = planet.transform.TransformPoint(meshMiddle);
        Vector3 raycastStart = meshMiddle + direction * 0.01f;
        
        //Debug.DrawRay(raycastStart, direction, Color.red, Mathf.Infinity);
        
        if (Physics.Raycast(raycastStart, direction, out hit, Mathf.Infinity))
        {
            Tile hitTile = hit.collider.GetComponent<Tile>();
           // tile.NextTile = hitTile;
            //hitTile.gameObject.SetActive(false);
            hitTile.GetComponent<Renderer>().material = tile.GetComponent<Renderer>().material;
        }
    }

    private Vector3 CalculateMeshMiddle(Mesh mesh)
    {
        Vector3 middle = Vector3.zero;

        foreach (var vertex in mesh.vertices)
        {
            middle += vertex;
        }

        middle = middle / mesh.vertices.Length;

        return middle;
    }

    private void SpawnCube(Vector3 position, Vector3 direction)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = Vector3.one * 0.2f;
        cube.transform.position = position;
        cube.transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    #endregion
}