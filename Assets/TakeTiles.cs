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
        var childrens = GetComponentsInChildren<MeshFilter>();
        
        foreach (var child in childrens)
        {
//            Vector3 meshMiddle = CalculateMeshMiddle(child.mesh);
            Vector3 meshMiddle = child.gameObject.GetComponent<Tile>().Center;
            Debug.Log($"{meshMiddle}");
            Vector3 direction = ( meshMiddle - transform.position).normalized;
            RaycastHit[] hits;
            SpawnCube(meshMiddle, direction);
            hits = Physics.RaycastAll(transform.position, direction, 100);
            foreach (RaycastHit raycastHit in hits)
            {
                Debug.Log($"{raycastHit.transform.GetComponent<MeshFilter>().mesh.name}");
            }
            
            Debug.Log($"==========");
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
