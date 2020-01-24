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
        //planeX.GenerateMesh(ref meshes);
 
        //Plane on Y axis
        Quad planeY = new Quad(new Vertex(new Vector3(0.5f, golden / 2, 0)),
                               new Vertex(new Vector3(0.5f, -golden / 2, 0)),
                               new Vertex(new Vector3(-0.5f, -golden / 2, 0)),
                               new Vertex(new Vector3(-0.5f, golden / 2, 0)));
        //planeY.GenerateMesh(ref meshes);
        
        //Plane on Z axis
        Quad planeZ = new Quad(
                               new Vertex(new Vector3(0,0.5f,golden/2)),
                               new Vertex(new Vector3(0,0.5f,-golden/2)),
                               new Vertex(new Vector3(0,-0.5f,-golden/2)),
                               new Vertex(new Vector3(0,-0.5f,golden/2)));

        //planeZ.GenerateMesh(ref meshes);

        triangles.Add(new Triangle(planeY.A, planeZ.B, planeY.D));
        triangles.Add(new Triangle(planeY.A, planeY.D, planeZ.A));
        triangles.Add(new Triangle(planeY.A, planeX.A, planeX.B));
        triangles.Add(new Triangle(planeY.A, planeZ.A, planeX.A));
        triangles.Add(new Triangle(planeZ.B, planeY.A, planeX.B));
        
        triangles.Add(new Triangle(planeZ.A, planeZ.D, planeX.A));
        triangles.Add(new Triangle(planeZ.B, planeX.B, planeZ.C));
        triangles.Add(new Triangle(planeX.B,planeX.A,planeY.B));
        triangles.Add(new Triangle(planeX.B,  planeY.B, planeZ.C));
        
        triangles.Add(new Triangle(planeY.C, planeY.B,planeZ.D));
        triangles.Add(new Triangle(planeX.C, planeY.C, planeX.D));
        triangles.Add(new Triangle(planeX.C, planeZ.C, planeY.C));        
        triangles.Add(new Triangle(planeX.C, planeZ.B, planeZ.C));
        triangles.Add(new Triangle(planeY.C, planeZ.C,  planeY.B));
        
        triangles.Add(new Triangle(planeX.D, planeZ.D, planeZ.A));
        triangles.Add(new Triangle(planeY.D, planeZ.B, planeX.C));
        triangles.Add(new Triangle(planeY.D, planeX.C, planeX.D));
        triangles.Add(new Triangle(planeY.D, planeX.D, planeZ.A));
        triangles.Add(new Triangle(planeX.D,  planeY.C, planeZ.D));
        triangles.Add(new Triangle(planeZ.D, planeY.B, planeX.A));

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        foreach (var triangle in triangles)
        {
           triangle.GenerateMesh();
        }
    }
}
