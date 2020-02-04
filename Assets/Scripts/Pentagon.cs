using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagon : MonoBehaviour
{
    private List<Triangle> triangles;
    private Vertex middle;
    private PlanetGenerator generator;

    public Pentagon(Vertex middle, Triangle a, Triangle b, Triangle c, Triangle d, Triangle e, PlanetGenerator planetGenerator)
    {
        triangles = new List<Triangle>();
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
        triangles.Add(d);
        triangles.Add(e);
        generator = planetGenerator;
        this.middle = middle;
        //middle.Truncate();
    }

    public void Truncate()
    {
        middle.Truncate();
    }

    public void GenerateMesh()
    {
        GameObject gameObject = new GameObject("Pentagon");
        gameObject.transform.parent = generator.transform;
        //gameObject.transform.position = middle.Position;
        //gameObject.transform.LookAt(Vector3.zero);
        
        Mesh mesh = new Mesh();
        
        List<Vector3> allVertices = new List<Vector3>();

        foreach (var vertex in generator.vertices)
        {
            allVertices.Add(vertex.Position);
        }

        mesh.vertices = allVertices.ToArray();

        List<int> trianglesInts = new List<int>();
        foreach (var triangle in triangles)
        {
            trianglesInts.AddRange(triangle.GetVertices());
        }

        mesh.triangles = trianglesInts.ToArray();
        
        mesh.RecalculateNormals();
//        mesh.Optimize();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = generator.material;
    }
    
    
    public void TruncateVertex()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        int count = 0;
        foreach (var triangle in triangles)
        {
            foreach (int vertex in triangle.GetVertices())
            {
                Vector3 position = generator.vertices[vertex].Position;
                if (!position.Equals( middle.Position))
                {
                    count++;
                    x += position.x;
                    y += position.y;
                    z += position.z;
                }
            }
        }
        
        Vector3 truncatedVertex = new Vector3();

        truncatedVertex.x = x / count;
        truncatedVertex.y = y / count;
        truncatedVertex.z = z / count;

        middle.Position = truncatedVertex;
        GenerateMesh();
    }
}

