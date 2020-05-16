using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTiles : MonoBehaviour
{
    private Hexsphere hexsphere;
    
    void Start()
    {
        hexsphere = GetComponent<Hexsphere>();
        Physics.queriesHitBackfaces = true;
        CollectTiles();
    }

    private void Update()
    {
    }

    private void CollectTiles()
    {
        var children = GetComponentsInChildren<MeshFilter>();
        
        foreach (var child in children)
        {
            RaycastHit hit;
            Vector3 meshMiddle = CalculateMeshMiddle(child.mesh);
            Vector3 direction = ( meshMiddle - transform.position).normalized;
            //Vector3 raycastPoint = direction * 3;
            SpawnCube(meshMiddle, direction);
            Debug.DrawRay(meshMiddle,-direction, Color.red, 30f);
            if (Physics.Raycast(meshMiddle, direction, out hit, Mathf.Infinity))
            {
                Debug.Log($"{hit.collider.name}");
            }
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
}
