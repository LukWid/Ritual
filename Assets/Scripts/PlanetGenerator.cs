using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using Random = System.Random;

public class PlanetGenerator : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private int recursionLevel;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private GameObject hexTile;
    
    #endregion

    #region Private Fields

    private float radius = 1.0f;
    private List<Hexagon> hexes;
    private List<Pentagon> pentagons;
    private HashSet<Vector3> toTruncate;
    private List<Triangle> triangles = new List<Triangle>();
    private Dictionary<Vector3, List<Triangle>> vertexTriangles;
    private List<Vertex> vertices = new List<Vertex>();
    private static Vector3CoordComparer customComparer = new Vector3CoordComparer();

    #endregion

    #region Public Properties

    public List<Vertex> Vertices { get => vertices; set => vertices = value; }
    public Dictionary<Vector3, List<Triangle>> VertexTriangles { get => vertexTriangles; private set => vertexTriangles = value; }
    public GameObject HexTile { get => hexTile; set => hexTile = value; }
    
    public int RecursionLevel { get => recursionLevel; set => recursionLevel = value; }
    public Material DefaultMaterial { get => defaultMaterial; set => defaultMaterial = value; }
    public float Radius { get => radius; set => radius = value; }

    #endregion

    #region Unity Callbacks

    public void CreatePlanet()
    {
        VertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);
        toTruncate = new HashSet<Vector3>(customComparer);

        float golden = ( 1f + Mathf.Sqrt(5f) ) / 2f;

        //12 Vertices of ico sphere

        Vertices.Add(new Vertex(new Vector3(-1f, golden, 0f), this));
        Vertices.Add(new Vertex(new Vector3(1f, golden, 0f), this));
        Vertices.Add(new Vertex(new Vector3(-1f, -golden, 0f), this));
        Vertices.Add(new Vertex(new Vector3(1f, -golden, 0f), this));

        Vertices.Add(new Vertex(new Vector3(0f, -1f, golden), this));
        Vertices.Add(new Vertex(new Vector3(0f, 1f, golden), this));
        Vertices.Add(new Vertex(new Vector3(0f, -1f, -golden), this));
        Vertices.Add(new Vertex(new Vector3(0f, 1f, -golden), this));

        Vertices.Add(new Vertex(new Vector3(golden, 0f, -1f), this));
        Vertices.Add(new Vertex(new Vector3(golden, 0f, 1f), this));
        Vertices.Add(new Vertex(new Vector3(-golden, 0f, -1f), this));
        Vertices.Add(new Vertex(new Vector3(-golden, 0f, 1f), this));

        //20 triangles

        triangles.Add(new Triangle(0, 11, 5, triangles, this));
        triangles.Add(new Triangle(0, 5, 1, triangles, this));
        triangles.Add(new Triangle(0, 1, 7, triangles, this));
        triangles.Add(new Triangle(0, 7, 10, triangles, this));
        triangles.Add(new Triangle(0, 10, 11, triangles, this));

        triangles.Add(new Triangle(1, 5, 9, triangles, this));
        triangles.Add(new Triangle(5, 11, 4, triangles, this));
        triangles.Add(new Triangle(11, 10, 2, triangles, this));
        triangles.Add(new Triangle(10, 7, 6, triangles, this));
        triangles.Add(new Triangle(7, 1, 8, triangles, this));

        triangles.Add(new Triangle(3, 9, 4, triangles, this));
        triangles.Add(new Triangle(3, 4, 2, triangles, this));
        triangles.Add(new Triangle(3, 2, 6, triangles, this));
        triangles.Add(new Triangle(3, 6, 8, triangles, this));
        triangles.Add(new Triangle(3, 8, 9, triangles, this));

        triangles.Add(new Triangle(4, 9, 5, triangles, this));
        triangles.Add(new Triangle(2, 4, 11, triangles, this));
        triangles.Add(new Triangle(6, 2, 10, triangles, this));
        triangles.Add(new Triangle(8, 6, 7, triangles, this));
        triangles.Add(new Triangle(9, 8, 1, triangles, this));

        for (int i = 0; i < recursionLevel - 1; i++)
        {
            List<Triangle> triangles2 = new List<Triangle>();
            VertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);
            foreach (var triangle in triangles)
            {
                Vector3 v = new Vector3(( Vertices[triangle.v1].Position.x + Vertices[triangle.v2].Position.x ) / 2, ( Vertices[triangle.v1].Position.y + Vertices[triangle.v2].Position.y ) / 2,
                                        ( Vertices[triangle.v1].Position.z + Vertices[triangle.v2].Position.z ) / 2);
                Vertex a = new Vertex(v.normalized, this);
                Vertices.Add(a);

                Vector3 p = new Vector3(( Vertices[triangle.v2].Position.x + Vertices[triangle.v3].Position.x ) / 2, ( Vertices[triangle.v2].Position.y + Vertices[triangle.v3].Position.y ) / 2,
                                        ( Vertices[triangle.v2].Position.z + Vertices[triangle.v3].Position.z ) / 2);
                Vertex b = new Vertex(p.normalized, this);
                Vertices.Add(b);

                Vector3 o = new Vector3(( Vertices[triangle.v3].Position.x + Vertices[triangle.v1].Position.x ) / 2, ( Vertices[triangle.v3].Position.y + Vertices[triangle.v1].Position.y ) / 2,
                                        ( Vertices[triangle.v3].Position.z + Vertices[triangle.v1].Position.z ) / 2);
                Vertex c = new Vertex(o.normalized, this);
                Vertices.Add(c);

                triangles2.Add(new Triangle(triangle.v1, a.vertexIndex, c.vertexIndex, triangles2, this));
                triangles2.Add(new Triangle(a.vertexIndex, triangle.v2, b.vertexIndex, triangles2, this));
                triangles2.Add(new Triangle(c.vertexIndex, b.vertexIndex, triangle.v3, triangles2, this));
                triangles2.Add(new Triangle(c.vertexIndex, a.vertexIndex, b.vertexIndex, triangles2, this));
            }

            triangles = triangles2;
        }

        List<Triangle> newTriangles = new List<Triangle>();
        VertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);

        hexes = new List<Hexagon>();
        pentagons = new List<Pentagon>();

        foreach (var triangle in triangles)
        {
            Vertex a = new Vertex(Vector3.Lerp(Vertices[triangle.v1].Position, Vertices[triangle.v2].Position, 1 / 3f), this);
            Vertices.Add(a);
        
            Vertex c = new Vertex(Vector3.Lerp(Vertices[triangle.v1].Position, Vertices[triangle.v2].Position, 2 / 3f), this);
            Vertices.Add(c);
        
            Vertex e = new Vertex(Vector3.Lerp(Vertices[triangle.v3].Position, Vertices[triangle.v1].Position, 1 / 3f), this);
            Vertices.Add(e);
        
            Vertex b = new Vertex(Vector3.Lerp(Vertices[triangle.v3].Position, Vertices[triangle.v1].Position, 2 / 3f), this);
            Vertices.Add(b);
        
            Vertex f = new Vertex(Vector3.Lerp(Vertices[triangle.v2].Position, Vertices[triangle.v3].Position, 1 / 3f), this);
            Vertices.Add(f);
        
            Vertex g = new Vertex(Vector3.Lerp(Vertices[triangle.v2].Position, Vertices[triangle.v3].Position, 2 / 3f), this);
            Vertices.Add(g);
        
            Vertex d =
                new Vertex(new Vector3(( b.Position.x + a.Position.x + c.Position.x + f.Position.x + g.Position.x + e.Position.x ) / 6, ( b.Position.y + a.Position.y + c.Position.y + f.Position.y + g.Position.y + e.Position.y ) / 6, ( b.Position.z + a.Position.z + c.Position.z + f.Position.z + g.Position.z + e.Position.z ) / 6),
                           this);
        
            //d.Position = new Vector3((float) Math.Round(d.Position.x, 1), (float) Math.Round(d.Position.y, 1), (float) Math.Round(d.Position.z, 1));
            Vertices.Add(d);
        
            toTruncate.Add(Vertices[triangle.v1].Position);
            toTruncate.Add(Vertices[triangle.v2].Position);
            toTruncate.Add(Vertices[triangle.v3].Position);
        
            newTriangles.Add(new Triangle(triangle.v1, a.vertexIndex, b.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(f.vertexIndex, c.vertexIndex, triangle.v2, newTriangles, this));
            newTriangles.Add(new Triangle(e.vertexIndex, g.vertexIndex, triangle.v3, newTriangles, this));
        
            Triangle tri1 = new Triangle(a.vertexIndex, c.vertexIndex, d.vertexIndex, newTriangles, this);
            newTriangles.Add(tri1);
            Triangle tri2 = new Triangle(a.vertexIndex, d.vertexIndex, b.vertexIndex, newTriangles, this);
            newTriangles.Add(tri2);
            Triangle tri3 = new Triangle(b.vertexIndex, d.vertexIndex, e.vertexIndex, newTriangles, this);
            newTriangles.Add(tri3);
            Triangle tri4 = new Triangle(d.vertexIndex, g.vertexIndex, e.vertexIndex, newTriangles, this);
            newTriangles.Add(tri4);
            Triangle tri5 = new Triangle(d.vertexIndex, f.vertexIndex, g.vertexIndex, newTriangles, this);
            newTriangles.Add(tri5);
            Triangle tri6 = new Triangle(d.vertexIndex, c.vertexIndex, f.vertexIndex, newTriangles, this);
            newTriangles.Add(tri6);
        
            Hexagon hex = new Hexagon(d, tri1, tri2, tri3, tri4, tri5, tri6, this);
            hexes.Add(hex);
        }
        
        triangles = newTriangles;

        foreach (var position in toTruncate)
        {
            List<Triangle> triangles = VertexTriangles[position];
            if (triangles.Count == 6)
            {
                Vertex middle = FindVertex(position);
                Hexagon hex = new Hexagon(middle, VertexTriangles[middle.Position], this);
                hexes.Add(hex);
            }
        }
        
        CastIntoSphere();
        
        foreach (var hex in hexes)
        {
            hex.GenerateMesh();
        }

        TruncatePentagons();
        foreach (var pent in pentagons)
        {
           pent.GenerateMesh();
        }

        //float pentagonEdgeLength = pentagons[0].GetEdgeLength();
        //Debug.Log($"length = {pentagonEdgeLength}");
    }

    private void CastIntoSphere()
    {
        foreach (var vertex in vertices)
        {
            vertex.Position = vertex.Position.normalized * radius;
        }
    }

    #endregion

    #region Public Methods

    public int GetVertexIndex(Vector3 position)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            if (Vertices[i].Position.Equals(position))
            {
                return Vertices[i].vertexIndex;
            }
        }

        return -1;
    }

    #endregion

    #region Private Methods

    private void TruncatePentagons()
    {
        foreach (KeyValuePair<Vector3, List<Triangle>> entry in VertexTriangles)
        {
            if (entry.Value.Count == 5)
            {
                Pentagon pent = new Pentagon(FindVertex(entry.Key), entry.Value[0], entry.Value[1], entry.Value[2], entry.Value[3], entry.Value[4], this);
                pent.TruncateVertex();
                pentagons.Add(pent);
                //Vector3 truncated = TruncateVertex(entry.Key, entry.Value);
                //foreach (var vertex in Vertices)
                //{
                //    if (vertex.Position == entry.Key)
                //    {
                //        vertex.Position = truncated;
                //    }
                //}
            }
        }
    }

    private Vector3 TruncateVertex(Vector3 entryKey, List<Triangle> triangles)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        int count = 0;
        foreach (var triangle in triangles)
        {
            foreach (int vertex in triangle.GetVertices())
            {
                Vector3 position = Vertices[vertex].Position;
                if (position != entryKey)
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
        return truncatedVertex;
    }

    private Vertex FindVertex(Vector3 position)
    {
        foreach (var vertex in Vertices)
        {
            if (vertex.Position == position)
            {
                return vertex;
            }
        }

        return null;
    }
    
    private void CreateIndices(Vector3 position, float size, Color color)
    {
        GameObject gol = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gol.GetComponent<Renderer>().material.color = color;
        gol.transform.position = position;
        gol.transform.parent = transform;
        gol.transform.localScale = new Vector3(size, size, size);
    }
    #endregion
}