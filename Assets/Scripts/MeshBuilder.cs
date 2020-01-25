using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Shader shader;
    [SerializeField]
    private GameObject hexTile;
    [SerializeField]
    private int subdivisionLevel;
    
    #endregion

    #region Private Fields

    private int index;
    private List<Triangle> baseTriangles;
    private List<Triangle> allTriangles;
    private HashSet<Vertex> vertices;
    private float radius = 1;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        vertices = new HashSet<Vertex>();
        baseTriangles = new List<Triangle>();
        allTriangles = new List<Triangle>();

        float golden = (float) Constants.GoldenRatio;

        //Plane on X axis
        Quad planeX = CreateQuad(new Vector3(golden / 2, 0, 0.5f), new Vector3(golden / 2, 0, -0.5f), new Vector3(-golden / 2, 0, -0.5f), new Vector3(-golden / 2, 0, 0.5f));
        //planeX.GenerateMesh();

        //Plane on Y axis
        Quad planeY = CreateQuad(new Vector3(0.5f, golden / 2, 0), new Vector3(0.5f, -golden / 2, 0), new Vector3(-0.5f, -golden / 2, 0), new Vector3(-0.5f, golden / 2, 0));
        //planeY.GenerateMesh();

        //Plane on Z axis
        Quad planeZ = CreateQuad(new Vector3(0, 0.5f, golden / 2), new Vector3(0, 0.5f, -golden / 2), new Vector3(0, -0.5f, -golden / 2), new Vector3(0, -0.5f, golden / 2));

        //planeZ.GenerateMesh();

        baseTriangles.Add(CreateTriangle(planeY.A, planeZ.B, planeY.D));
        baseTriangles.Add(CreateTriangle(planeY.A, planeY.D, planeZ.A));
        baseTriangles.Add(CreateTriangle(planeY.A, planeX.A, planeX.B));
        baseTriangles.Add(CreateTriangle(planeY.A, planeZ.A, planeX.A));
        baseTriangles.Add(CreateTriangle(planeZ.B, planeY.A, planeX.B));

        baseTriangles.Add(CreateTriangle(planeZ.A, planeZ.D, planeX.A));
        baseTriangles.Add(CreateTriangle(planeZ.B, planeX.B, planeZ.C));
        baseTriangles.Add(CreateTriangle(planeX.B, planeX.A, planeY.B));
        baseTriangles.Add(CreateTriangle(planeX.B, planeY.B, planeZ.C));

        baseTriangles.Add(CreateTriangle(planeY.C, planeY.B, planeZ.D));
        baseTriangles.Add(CreateTriangle(planeX.C, planeY.C, planeX.D));
        baseTriangles.Add(CreateTriangle(planeX.C, planeZ.C, planeY.C));
        baseTriangles.Add(CreateTriangle(planeX.C, planeZ.B, planeZ.C));
        baseTriangles.Add(CreateTriangle(planeY.C, planeZ.C, planeY.B));

        baseTriangles.Add(CreateTriangle(planeX.D, planeZ.D, planeZ.A));
        baseTriangles.Add(CreateTriangle(planeY.D, planeZ.B, planeX.C));
        baseTriangles.Add(CreateTriangle(planeY.D, planeX.C, planeX.D));
        baseTriangles.Add(CreateTriangle(planeY.D, planeX.D, planeZ.A));
        baseTriangles.Add(CreateTriangle(planeX.D, planeY.C, planeZ.D));
        baseTriangles.Add(CreateTriangle(planeZ.D, planeY.B, planeX.A));

        GetTriangles();
        GetVertices(vertices);
        CastVerticesOnSphere();
        RecalculateMesh();
    }

    private void RecalculateMesh()
    {
        foreach (var triangle in allTriangles)
        {
            triangle.Recalculate();
        }
    }

    private void CastVerticesOnSphere()
    {
        foreach (var vertex in vertices)
        {
            vertex.Position = CastOnSphere(vertex);
        }
    }

    private void GetTriangles()
    {
        foreach (var triangle in baseTriangles)
        {
            triangle.GetTriangles(allTriangles);
        }
    }
    
    
    private Vector3 CastOnSphere(Vertex vertex)
    {
        Vector3 center = transform.position;
        Vector3 direction = vertex.Position - center;
        direction.Normalize();
        vertex.Position = direction * radius;
       
        return vertex.Position;
    }

    #endregion

    #region Private Methods

    private void GetVertices(HashSet<Vertex> vertices)
    {
        foreach (var triangle in baseTriangles)
        {
            triangle.GetVertices(vertices);
        }
    }

    private Triangle CreateTriangle(Vertex a, Vertex b, Vertex c)
    {
        GameObject triangle = new GameObject($"Triangle {index}");
        triangle.transform.parent = transform;
        Triangle triangleScript = triangle.AddComponent<Triangle>();
        triangleScript.Initialize(a, b, c, triangle, subdivisionLevel);
        index++;
        return triangleScript;
    }

    private Quad CreateQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Vertex vertexA = new Vertex(a);
        Vertex vertexB = new Vertex(b);
        Vertex vertexC = new Vertex(c);
        Vertex vertexD = new Vertex(d);

        return new Quad(vertexA, vertexB, vertexC, vertexD);
    }

    private void GetNeighbours()
    {
        foreach (var vertex in vertices)
        {
            Debug.Log("============");
            vertex.FiguresWithCommonVertex.ForEach(i => Debug.Log(i.name));
        }
    }

    private void CreateIndicator(Vector3 position)
    {
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        indicator.transform.position = position;
        indicator.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    #endregion
}