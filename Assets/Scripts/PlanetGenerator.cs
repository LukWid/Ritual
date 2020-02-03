using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    #region Private Fields

    private float radius = 1.0f;

    #endregion

    #region  Constants

    public static Vector3CoordComparer customComparer = new Vector3CoordComparer();

    #endregion

    public GameObject hexTile;
    public Material material;
    public int recursionLevel;
    public List<Triangle> triangles = new List<Triangle>();
    public Dictionary<Vector3, List<Triangle>> vertexTriangles;
    public List<Vertex> vertices = new List<Vertex>();

    #region Unity Callbacks

    private void Start()
    {
        vertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);
        // float golden = (float) Constants.GoldenRatio;
        float golden = ( 1f + Mathf.Sqrt(5f) ) / 2f;

        //12 Vertices of ico sphere
        //-1f,  t,  0f)

        vertices.Add(new Vertex(new Vector3(-1f, golden, 0f).normalized, this));
        vertices.Add(new Vertex(new Vector3(1f, golden, 0f).normalized, this));
        vertices.Add(new Vertex(new Vector3(-1f, -golden, 0f).normalized, this));
        vertices.Add(new Vertex(new Vector3(1f, -golden, 0f).normalized, this));

        vertices.Add(new Vertex(new Vector3(0f, -1f, golden).normalized, this));
        vertices.Add(new Vertex(new Vector3(0f, 1f, golden).normalized, this));
        vertices.Add(new Vertex(new Vector3(0f, -1f, -golden).normalized, this));
        vertices.Add(new Vertex(new Vector3(0f, 1f, -golden).normalized, this));

        vertices.Add(new Vertex(new Vector3(golden, 0f, -1f).normalized, this));
        vertices.Add(new Vertex(new Vector3(golden, 0f, 1f).normalized, this));
        vertices.Add(new Vertex(new Vector3(-golden, 0f, -1f).normalized, this));
        vertices.Add(new Vertex(new Vector3(-golden, 0f, 1f).normalized, this));

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
            vertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);
            foreach (var triangle in triangles)
            {
                //Vertex a = new Vertex(Vector3.Lerp(vertices[triangle.v1].Position, vertices[triangle.v2].Position, 0.5f).normalized, this);
                Vector3 v = new Vector3(( vertices[triangle.v1].Position.x + vertices[triangle.v2].Position.x ) / 2, ( vertices[triangle.v1].Position.y + vertices[triangle.v2].Position.y ) / 2,
                                        ( vertices[triangle.v1].Position.z + vertices[triangle.v2].Position.z ) / 2);
                Vertex a = new Vertex(v.normalized, this);
                vertices.Add(a);

                Vector3 p = new Vector3(( vertices[triangle.v2].Position.x + vertices[triangle.v3].Position.x ) / 2, ( vertices[triangle.v2].Position.y + vertices[triangle.v3].Position.y ) / 2,
                                        ( vertices[triangle.v2].Position.z + vertices[triangle.v3].Position.z ) / 2);
                Vertex b = new Vertex(p.normalized, this);
                vertices.Add(b);

                Vector3 o = new Vector3(( vertices[triangle.v3].Position.x + vertices[triangle.v1].Position.x ) / 2, ( vertices[triangle.v3].Position.y + vertices[triangle.v1].Position.y ) / 2,
                                        ( vertices[triangle.v3].Position.z + vertices[triangle.v1].Position.z ) / 2);
                Vertex c = new Vertex(o.normalized, this);
                vertices.Add(c);

                triangles2.Add(new Triangle(triangle.v1, a.vertexIndex, c.vertexIndex, triangles2, this));
                triangles2.Add(new Triangle(a.vertexIndex, triangle.v2, b.vertexIndex, triangles2, this));
                triangles2.Add(new Triangle(c.vertexIndex, b.vertexIndex, triangle.v3, triangles2, this));
                triangles2.Add(new Triangle(c.vertexIndex, a.vertexIndex, b.vertexIndex, triangles2, this));
            }

            triangles = triangles2;
        }

        List<Triangle> newTriangles = new List<Triangle>();
        vertexTriangles = new Dictionary<Vector3, List<Triangle>>(customComparer);

        foreach (var triangle in triangles)
        {
            Vertex a = new Vertex(Vector3.Lerp(vertices[triangle.v1].Position, vertices[triangle.v2].Position, 1 / 3f).normalized, this);
            vertices.Add(a);

            Vertex c = new Vertex(Vector3.Lerp(vertices[triangle.v1].Position, vertices[triangle.v2].Position, 2 / 3f).normalized, this);
            vertices.Add(c);

            Vertex e = new Vertex(Vector3.Lerp(vertices[triangle.v3].Position, vertices[triangle.v1].Position, 1 / 3f).normalized, this);
            vertices.Add(e);

            Vertex b = new Vertex(Vector3.Lerp(vertices[triangle.v3].Position, vertices[triangle.v1].Position, 2 / 3f).normalized, this);
            vertices.Add(b);

            Vertex f = new Vertex(Vector3.Lerp(vertices[triangle.v2].Position, vertices[triangle.v3].Position, 1 / 3f).normalized, this);
            vertices.Add(f);

            Vertex g = new Vertex(Vector3.Lerp(vertices[triangle.v2].Position, vertices[triangle.v3].Position, 2 / 3f).normalized, this);
            vertices.Add(g);

            Vertex d =
                new
                    Vertex(new Vector3(( vertices[triangle.v1].Position.x + vertices[triangle.v2].Position.x + vertices[triangle.v3].Position.x ) / 3, ( vertices[triangle.v1].Position.y + vertices[triangle.v2].Position.y + vertices[triangle.v3].Position.y ) / 3, ( vertices[triangle.v1].Position.z + vertices[triangle.v2].Position.z + vertices[triangle.v3].Position.z ) / 3).normalized,
                           this);

            //Vertex d = new Vertex( new Vector3((b.Position.x + a.Position.x + c.Position.x + f.Position.x + g.Position.x + e.Position.x)/6, 
            //                         (b.Position.y + a.Position.y + c.Position.y + f.Position.y + g.Position.y + e.Position.y)/6,
            //                         (b.Position.z + a.Position.z + c.Position.z + f.Position.z + g.Position.z + e.Position.z)/6
            //                         ), this);

            vertices.Add(d);

            newTriangles.Add(new Triangle(triangle.v1, a.vertexIndex, b.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(a.vertexIndex, c.vertexIndex, d.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(a.vertexIndex, d.vertexIndex, b.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(b.vertexIndex, d.vertexIndex, e.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(d.vertexIndex, g.vertexIndex, e.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(d.vertexIndex, f.vertexIndex, g.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(d.vertexIndex, c.vertexIndex, f.vertexIndex, newTriangles, this));
            newTriangles.Add(new Triangle(f.vertexIndex, c.vertexIndex, triangle.v2, newTriangles, this));
            newTriangles.Add(new Triangle(e.vertexIndex, g.vertexIndex, triangle.v3, newTriangles, this));
        }

        triangles = newTriangles;

        foreach (KeyValuePair<Vector3, List<Triangle>> entry in vertexTriangles)
        {
            Debug.Log($"{entry.Value.Count}");
            // if (entry.Value.Count == 3)
            {
                //    Debug.Log($"{entry.Key.x}:{entry.Key.y}:{entry.Key.z}");
            }
        }

        BuildMesh();
    }

    #endregion

    #region Private Methods

    //private void AddToDictionary(int index, Dictionary<Vector3, List<Triangle>> vertexTriangles, List<Vertex> vertices)
    //{
    //    if (vertexTriangles.ContainsKey(vertices[index].Position))
    //    {
    //        vertexTriangles[vertices[index].Position].Add(this);
    //    }
    //    else
    //    {
    //        vertexTriangles[vertices[index].Position] = new List<Triangle>();
    //        vertexTriangles[vertices[index].Position].Add(this);
    //    }
    //}

    private void SpawnHex(Triangle triangle)
    {
        Vector3 v1 = vertices[triangle.v1].Position;
        Vector3 v2 = vertices[triangle.v2].Position;
        Vector3 v3 = vertices[triangle.v3].Position;

        Vector3 middle = new Vector3(( v1.x + v2.x + v3.x ) / 3, ( v1.y + v2.y + v3.y ) / 3, ( v1.z + v2.z + v3.z ) / 3);

        GameObject go = Instantiate(hexTile);
        go.transform.localScale = Vector3.one * .3f;
        go.transform.position = middle;
        go.transform.LookAt(Vector3.zero);
        go.transform.Rotate(transform.right, 90);
        //go.transform.Rotate(transform.up, 90);
    }

    private void BuildMesh()
    {
        Mesh face = new Mesh();

        List<Vector3> allVertices = new List<Vector3>();
        foreach (var vertex in vertices)
        {
            allVertices.Add(vertex.Position);
        }

        face.vertices = allVertices.ToArray();

        List<int> trianglesInts = new List<int>();
        foreach (var triangle in triangles)
        {
            trianglesInts.AddRange(triangle.GetVertices());
        }

        face.triangles = trianglesInts.ToArray();

        face.RecalculateNormals();
        face.Optimize();

        GetComponent<MeshFilter>().mesh = face;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    #endregion

    public struct Tris
    {
        public int v1;
        public int v2;
        public int v3;

        #region Constructors

        public Tris(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        #endregion
    }
}