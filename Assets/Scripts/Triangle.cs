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

    private int targetSubdivision;
    private int actualSubdivision;
    private List<Triangle> dividedTriangles;

    #endregion

    #region Public Methods
    
    public void Initialize(Vertex a, Vertex b, Vertex c, GameObject gameObject, int targetSubdivision = 0, int actualSubdivision = 0)
    {
        //Debug.Log($"{actualSubdivision}");
        dividedTriangles = new List<Triangle>();
        
        this.a = a;
        this.b = b;
        this.c = c;
        
        this.targetSubdivision = targetSubdivision;
        this.actualSubdivision = actualSubdivision;

        a.FiguresWithCommonVertex.Add(this);
        b.FiguresWithCommonVertex.Add(this);
        c.FiguresWithCommonVertex.Add(this);

        ab = new Edge(a, b);
        bc = new Edge(b, c);
        ca = new Edge(c, a);

        GenerateMesh(gameObject);
    }

    public void GetVertices(HashSet<Vertex> vertices)
    {
        if (actualSubdivision == targetSubdivision)
        {
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
        }
        else
        {
            foreach (var triangle in dividedTriangles)
            {
                triangle.GetVertices(vertices);
            }
        }
    }

    public void GetTriangles(List<Triangle> triangles)
    {
        if (actualSubdivision == targetSubdivision)
        {
            triangles.Add(this);
        }
        else
        {
            foreach (var triangle in dividedTriangles)
            {
                triangle.GetTriangles(triangles);
            }
        }
    }

    public override void GenerateMesh(GameObject gameObject)
    {
        if (actualSubdivision == targetSubdivision)
        {
            Mesh shape = GenerateTriangleFace();
            GenerateShape("triangle", shape, gameObject);
        }
        else if(actualSubdivision < targetSubdivision)
        {
            dividedTriangles = Subdivide();
        }
    }

    public void Recalculate()
    {
        if (actualSubdivision == targetSubdivision)
        {
            Mesh shape = GenerateTriangleFace();
            RecalculateShape(shape);
        }
        else
        {
            foreach (var triangle in dividedTriangles)
            {
                Mesh shape = triangle.GenerateTriangleFace();
                if (triangle.GetComponent<MeshFilter>() != null)
                {
                    triangle.RecalculateShape(shape);
                }
            }
        }
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

    private List<Triangle> Subdivide()
    {
        List<Triangle> triangles = new List<Triangle>();

        Vertex[] dividedAb = ab.DivideEdge(2);
        Vertex[] dividedBc = bc.DivideEdge(2);
        Vertex[] dividedCa = ca.DivideEdge(2);

        GameObject upTriangle = new GameObject("up");
        Triangle triangleUp = upTriangle.AddComponent<Triangle>();
        triangleUp.Initialize(dividedAb[0], dividedAb[1], dividedCa[1],upTriangle,targetSubdivision, actualSubdivision + 1);
        upTriangle.transform.parent = transform;
        triangles.Add(triangleUp);

        GameObject downTriangle = new GameObject("down");
        Triangle triangleDown = downTriangle.AddComponent<Triangle>();
        triangleDown.Initialize(dividedAb[1], dividedBc[1], dividedCa[1], downTriangle, targetSubdivision, actualSubdivision + 1);
        downTriangle.transform.parent = transform;
        triangles.Add(triangleDown);

        GameObject leftTriangle = new GameObject("left");
        Triangle triangleLeft = leftTriangle.AddComponent<Triangle>();
        triangleLeft.Initialize(dividedBc[1], dividedBc[2], dividedCa[1], leftTriangle, targetSubdivision, actualSubdivision + 1);
        leftTriangle.transform.parent = transform;
        triangles.Add(triangleLeft);

        GameObject rightTriangle = new GameObject("right");
        Triangle triangleRight = rightTriangle.AddComponent<Triangle>();
        triangleRight.Initialize(dividedAb[1], dividedBc[0], dividedBc[1], rightTriangle, targetSubdivision, actualSubdivision + 1);
        rightTriangle.transform.parent = transform;
        triangles.Add(triangleRight);

        return triangles;
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