using System.Collections.Generic;
using UnityEngine;

public class Triangle : GeometricShape
{
    #region Private Fields

    private Vertex a;
    private Vertex b;
    private Vertex c;

    private Edge ab;
    private Edge bc;
    private Edge ca;

    private int subdivide;
    private List<Triangle> dividedTriangles;

    #endregion

    #region Public Methods

    public void Initialize(Vertex a, Vertex b, Vertex c, int subdivide = 0)
    {
        this.a = a;
        this.b = b;
        this.c = c;

        a.FiguresWithCommonVertex.Add(this);
        b.FiguresWithCommonVertex.Add(this);
        c.FiguresWithCommonVertex.Add(this);

        ab = new Edge(a, b);
        bc = new Edge(b, c);
        ca = new Edge(c, a);

        this.subdivide = subdivide;
        Subdivide(3);
    }

    public override void GenerateMesh()
    {
        Mesh shape = GenerateTriangleFace();
        GenerateShape("triangle", shape);
    }

    public Quaternion GetDirection()
    {
        Vertex middle = GetMiddle();

        Vector3 direction = a.Position - middle.Position;
        Vector3 secondDirection = c.Position - middle.Position;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.Cross(secondDirection, direction));
        return rotation;
    }

    public Vertex GetMiddle()
    {
        Vertex middlePointOnBasis = new Vertex(Vector3.Lerp(b.Position, c.Position, 0.5f));
        Edge triangleHeight = new Edge(middlePointOnBasis, a);
        Vertex middlePoint = triangleHeight.DivideEdge(3)[1];

        //Draw point
        // GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // indicator.transform.localScale = new Vector3(0.02f,0.02f,0.02f);
        // indicator.transform.position = middlePoint.Position;

        return middlePoint;
    }

    #endregion

    #region Private Methods

    private void Subdivide(int frequency)
    {
        //GetMiddle();
        ab.DivideEdge(frequency);
        bc.DivideEdge(frequency);
        ca.DivideEdge(frequency);
    }

    private Mesh GenerateTriangleFace()
    {
        Mesh face = new Mesh();

        Vector3[] vertices = new Vector3[3] { a.Position, b.Position, c.Position };
        face.vertices = vertices;

        int[] triangle = new int[3] { 0, 1, 2 };
        face.triangles = triangle;

        face.RecalculateNormals();

        return face;
    }

    #endregion
}