using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{

    [SerializeField]
    private Shader shader;
    [SerializeField]
    private GameObject hexTile;
    
    private List<Triangle> triangles;
    
    void Start()
    {
        triangles = new List<Triangle>();

        float golden = (float)Constants.GoldenRatio;

        //Plane on X axis
        Quad planeX = new Quad(new Vertex(new Vector3(golden/2, 0,0.5f)),
                                new Vertex(new Vector3(golden/2, 0,-0.5f)),
                                new Vertex(new Vector3(-golden/2, 0,-0.5f)),
                                new Vertex(new Vector3(-golden/2, 0,0.5f))
                               );
        //planeX.GenerateMesh();
 
        //Plane on Y axis
        Quad planeY = new Quad(new Vertex(new Vector3(0.5f, golden / 2, 0)),
                               new Vertex(new Vector3(0.5f, -golden / 2, 0)),
                               new Vertex(new Vector3(-0.5f, -golden / 2, 0)),
                               new Vertex(new Vector3(-0.5f, golden / 2, 0)));
        //planeY.GenerateMesh();
        
        //Plane on Z axis
        Quad planeZ = new Quad(
                               new Vertex(new Vector3(0,0.5f,golden/2)),
                               new Vertex(new Vector3(0,0.5f,-golden/2)),
                               new Vertex(new Vector3(0,-0.5f,-golden/2)),
                               new Vertex(new Vector3(0,-0.5f,golden/2)));

        //planeZ.GenerateMesh();

        triangles.Add(CreateTriangle(planeY.A, planeZ.B, planeY.D));
        triangles.Add(CreateTriangle(planeY.A, planeY.D, planeZ.A));
        triangles.Add(CreateTriangle(planeY.A, planeX.A, planeX.B));
        triangles.Add(CreateTriangle(planeY.A, planeZ.A, planeX.A));
        triangles.Add(CreateTriangle(planeZ.B, planeY.A, planeX.B));
        
        triangles.Add(CreateTriangle(planeZ.A, planeZ.D, planeX.A));
        triangles.Add(CreateTriangle(planeZ.B, planeX.B, planeZ.C));
        triangles.Add(CreateTriangle(planeX.B,planeX.A,planeY.B));
        triangles.Add(CreateTriangle(planeX.B,  planeY.B, planeZ.C));
        
        triangles.Add(CreateTriangle(planeY.C, planeY.B,planeZ.D));
        triangles.Add(CreateTriangle(planeX.C, planeY.C, planeX.D));
        triangles.Add(CreateTriangle(planeX.C, planeZ.C, planeY.C));        
        triangles.Add(CreateTriangle(planeX.C, planeZ.B, planeZ.C));
        triangles.Add(CreateTriangle(planeY.C, planeZ.C,  planeY.B));
        
        triangles.Add(CreateTriangle(planeX.D, planeZ.D, planeZ.A));
        triangles.Add(CreateTriangle(planeY.D, planeZ.B, planeX.C));
        triangles.Add(CreateTriangle(planeY.D, planeX.C, planeX.D));
        triangles.Add(CreateTriangle(planeY.D, planeX.D, planeZ.A));
        triangles.Add(CreateTriangle(planeX.D,  planeY.C, planeZ.D));
        triangles.Add(CreateTriangle(planeZ.D, planeY.B, planeX.A));

        GenerateMesh();
    }

    private Triangle CreateTriangle(Vertex a, Vertex b, Vertex c)
    {
        GameObject triangle = new GameObject("Triangle");
        triangle.transform.parent = transform;
        Triangle triangleScript = triangle.AddComponent<Triangle>();
        triangleScript.Initialize(a, b, c);
        return triangleScript;
    }

    private void GenerateMesh()
    {
        foreach (var triangle in triangles)
        {
           triangle.GenerateMesh();
        }
    }
}
