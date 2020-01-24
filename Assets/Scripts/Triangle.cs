using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    private Vertex a;
    private Vertex b;
    private Vertex c;

    private Edge ab;
    private Edge bc;
    private Edge ca;
    
    private int subdivide;

    private List<Triangle> dividedTriangles;
    
    public Triangle(Vertex a, Vertex b, Vertex c, int subdivide = 0)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        
        ab = new Edge(a,b);
        bc = new Edge(b,c);
        ca = new Edge(c,a);

        this.subdivide = subdivide;

        //if (subdivide > 0)
        {
            Subdivide(3);
        }
    }

    //In clockwise direction
    public Triangle(Edge ab, Edge bc, Edge ca, int subdivide = 0)
    {
        this.ab = ab;
        this.bc = bc;
        this.ca = ca;

        this.a = ab.A;
        this.b = bc.A;
        this.c = ca.A;

        this.subdivide = subdivide;
        
       // if (subdivide > 0)
        {
            Subdivide(4);
        }
    }

    private void Subdivide(int frequency)
    {
        //GetMiddle();
        ab.DivideEdge(frequency);
        bc.DivideEdge(frequency);
        ca.DivideEdge(frequency);
    }

    public Mesh GenerateMesh()
    {
        
        return GenerateTriangleFace();
        
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

    public Quaternion GetDirection()
    {
        Vertex middle = GetMiddle();

        Vector3 direction = ( a.Position - middle.Position );
        Vector3 secondDirection = ( c.Position - middle.Position );
        
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.Cross(secondDirection,direction));
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
}
