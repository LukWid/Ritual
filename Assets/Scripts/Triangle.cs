using System.Collections.Generic;
using System.Linq;
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
    
    public List<Triangle> dividedTriangles;
    public List<Triangle> neighbours;
    
    public List<Triangle> Neighbours { get => neighbours; set => neighbours = value; }
    public List<Vertex> Vertices {get => new List<Vertex>(){a,b,c};}

    public int v1;
    public int v2;
    public int v3;

    public int trisIndex;
    
    private PlanetGenerator planetGenerator;
    
    #endregion

    #region Public Methods

    public Triangle(int v1, int v2, int v3, List<Triangle> allTriangles, PlanetGenerator planetGenerator)
    {
        this.v1 = v1; 
        this.v2 = v2; 
        this.v3 = v3;

        trisIndex = allTriangles.Count;
        
        AddToDictionary(v1, planetGenerator.vertexTriangles, planetGenerator.vertices);
        AddToDictionary(v2, planetGenerator.vertexTriangles, planetGenerator.vertices);
        AddToDictionary(v3, planetGenerator.vertexTriangles, planetGenerator.vertices);
    }

    private void AddToDictionary(int index, Dictionary<Vector3, List<Triangle>> vertexTriangles, List<Vertex> vertices)
    {
        if (vertexTriangles.ContainsKey(vertices[index].Position))
        {
            vertexTriangles[vertices[index].Position].Add(this);
        }
        else
        {
            vertexTriangles[vertices[index].Position] = new List<Triangle>();
            vertexTriangles[vertices[index].Position].Add(this);
        }
    }
    
    public int[] GetVertices()
    {
        return new int[3]{ v1,v2,v3};
    }

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

    public void GetVertices(List<Vertex> vertices)
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
    
    public void GetCommonVertices(List<Vertex> vertices)
    {
        if (actualSubdivision == targetSubdivision)
        {
            VertexCollected(vertices, a);
            VertexCollected(vertices, b);
            VertexCollected(vertices, c);
        }
        else
        {
            foreach (var triangle in dividedTriangles)
            {
                triangle.GetCommonVertices(vertices);
            }
        }
    }
    
    public void GetEdges(List<Edge> edges)
    {
        if (actualSubdivision == targetSubdivision)
        {
            EdgeCollected(edges, ab);
            EdgeCollected(edges, bc);
            EdgeCollected(edges, ca);
        }
        else
        {
            foreach (var triangle in dividedTriangles)
            {
                triangle.GetEdges(edges);
            }
        }
    }

    private void EdgeCollected(List<Edge> edges, Edge toFind)
    {
        bool isInList = edges.Contains(toFind);

        if (isInList)
        {
            int index = edges.IndexOf(toFind);
            edges[index].FiguresWithCommonEdge.Add(this);
        }
        else
        {
            toFind.FiguresWithCommonEdge.Add(this);
            edges.Add(toFind);
        }
    }

    private void VertexCollected(List<Vertex> vertices, Vertex toFind)
    {
        bool isInList = vertices.Contains(toFind);

        if (isInList)
        {
            int index = vertices.IndexOf(toFind);
            vertices[index].FiguresWithCommonVertex.Add(this);
        }
        else
        {
            toFind.FiguresWithCommonVertex.Add(this);
            vertices.Add(toFind);
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

        if(actualSubdivision < targetSubdivision)
        {
            dividedTriangles = Subdivide();
        }
        else
        {
            //dividedTriangles = SubdivideForHex();
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
        Vertex middlePoint = triangleHeight.DivideEdge(3, false)[1];

        //Draw point
        //GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //indicator.name = "Middle";
        //indicator.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        //indicator.transform.position = middlePoint.Position;

        return middlePoint;
    }

    #endregion

    #region Private Methods

    private List<Triangle> Subdivide()
    {
        List<Triangle> triangles = new List<Triangle>();

        Vertex[] dividedAb = ab.DivideEdge(2, false);
        Vertex[] dividedBc = bc.DivideEdge(2, false);
        Vertex[] dividedCa = ca.DivideEdge(2, false);

        CreateNewTriangle(dividedAb[0], dividedAb[1], dividedCa[1], "up", triangles);
        CreateNewTriangle(dividedAb[1], dividedBc[1], dividedCa[1], "down", triangles);
        CreateNewTriangle(dividedBc[1], dividedBc[2], dividedCa[1],"left", triangles);
        CreateNewTriangle(dividedAb[1], dividedBc[0], dividedBc[1], "right", triangles);
        
        return triangles;
    }

    public List<Triangle> SubdivideForHex()
    {
        List<Triangle> triangles = new List<Triangle>();
        
        Vertex[] dividedAb = ab.DivideEdge(3, false);
        Vertex[] dividedBc = bc.DivideEdge(3, false);
        Vertex[] dividedCa = ca.DivideEdge(3, false);

        Vertex middle = GetMiddle();
        middle.Position = middle.Position * 30;
        
        CreateNewTriangle(dividedAb[0], dividedAb[1], dividedCa[2], "up", triangles);
        CreateNewTriangle(dividedCa[1], dividedBc[2], dividedCa[0], "left", triangles);
        CreateNewTriangle(dividedAb[2], dividedBc[0], dividedBc[1], "right", triangles);
       
        CreateNewTriangle(dividedAb[1], middle, dividedCa[2], "innerUp", triangles);
        CreateNewTriangle(dividedCa[2], middle,dividedCa[1], "middleLeft", triangles);
        CreateNewTriangle(dividedAb[1], dividedAb[2], middle, "middleRight", triangles);
        CreateNewTriangle(middle, dividedBc[2], dividedCa[1], "innerLeft", triangles);
        CreateNewTriangle(middle, dividedBc[1], dividedBc[2], "middleDown", triangles);
        CreateNewTriangle(middle, dividedAb[2], dividedBc[1], "innerRight", triangles);
        
        Hexagon hex = new Hexagon(middle, triangles[3],triangles[4],triangles[5],triangles[6],triangles[7],triangles[8]);
        hex.TruncateHex();
        
        return triangles;
    }
 
    private void CreateNewTriangle(Vertex a, Vertex b, Vertex c, string name, List<Triangle> triangles)
    {
        GameObject triangleGameObject = new GameObject($"{gameObject.name} {name}");
        Triangle triangleScript = triangleGameObject.AddComponent<Triangle>();
        triangleScript.Initialize(a, b, c, triangleGameObject, targetSubdivision, actualSubdivision + 1);
        triangleGameObject.transform.parent = transform;
        triangles.Add(triangleScript);
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