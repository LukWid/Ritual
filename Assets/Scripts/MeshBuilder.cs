using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Shader shader;
    [SerializeField]
    private GameObject hexTile;

    #endregion

    #region Private Fields

    private int index;
    private List<Triangle> triangles;
    private List<Vertex> vertices;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        triangles = new List<Triangle>();
        vertices = new List<Vertex>();

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

        triangles.Add(CreateTriangle(planeY.A, planeZ.B, planeY.D));
        triangles.Add(CreateTriangle(planeY.A, planeY.D, planeZ.A));
        triangles.Add(CreateTriangle(planeY.A, planeX.A, planeX.B));
        triangles.Add(CreateTriangle(planeY.A, planeZ.A, planeX.A));
        triangles.Add(CreateTriangle(planeZ.B, planeY.A, planeX.B));

        triangles.Add(CreateTriangle(planeZ.A, planeZ.D, planeX.A));
        triangles.Add(CreateTriangle(planeZ.B, planeX.B, planeZ.C));
        triangles.Add(CreateTriangle(planeX.B, planeX.A, planeY.B));
        triangles.Add(CreateTriangle(planeX.B, planeY.B, planeZ.C));

        triangles.Add(CreateTriangle(planeY.C, planeY.B, planeZ.D));
        triangles.Add(CreateTriangle(planeX.C, planeY.C, planeX.D));
        triangles.Add(CreateTriangle(planeX.C, planeZ.C, planeY.C));
        triangles.Add(CreateTriangle(planeX.C, planeZ.B, planeZ.C));
        triangles.Add(CreateTriangle(planeY.C, planeZ.C, planeY.B));

        triangles.Add(CreateTriangle(planeX.D, planeZ.D, planeZ.A));
        triangles.Add(CreateTriangle(planeY.D, planeZ.B, planeX.C));
        triangles.Add(CreateTriangle(planeY.D, planeX.C, planeX.D));
        triangles.Add(CreateTriangle(planeY.D, planeX.D, planeZ.A));
        triangles.Add(CreateTriangle(planeX.D, planeY.C, planeZ.D));
        triangles.Add(CreateTriangle(planeZ.D, planeY.B, planeX.A));

        GetNeighbours();
        GenerateMesh();
    }

    #endregion

    #region Private Methods

    private Triangle CreateTriangle(Vertex a, Vertex b, Vertex c)
    {
        GameObject triangle = new GameObject($"Triangle {index}");
        triangle.transform.parent = transform;
        Triangle triangleScript = triangle.AddComponent<Triangle>();
        triangleScript.Initialize(a, b, c);
        index++;
        return triangleScript;
    }

    private Quad CreateQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Vertex vertexA = new Vertex(a);
        Vertex vertexB = new Vertex(b);
        Vertex vertexC = new Vertex(c);
        Vertex vertexD = new Vertex(d);

        vertices.Add(vertexA);
        vertices.Add(vertexB);
        vertices.Add(vertexC);
        vertices.Add(vertexD);

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

    private void GenerateMesh()
    {
        foreach (var triangle in triangles)
        {
            triangle.GenerateMesh();
        }
    }

    #endregion
}