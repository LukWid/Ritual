using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Hexsphere : MonoBehaviour
{
    #region Serialized Fields
    
    private List<Vector3> centers = new List<Vector3>();

    [SerializeField, Range(1, 5)]
    private int detailLevel;

    [SerializeField]
    private Material material;

    [SerializeField]
    private int TileCount;

    #endregion

    #region Private Fields

    private float radialScale;
    private float maxEdgeLength;
    private float maxTileRadius;
    private float worldRadius;

    private List<Vector3> vectors = new List<Vector3>();
    private List<int> indices = new List<int>();

    private List<GameObject> Tiles = new List<GameObject>();

    #endregion

    private List<Vector3> Centers { get => centers; set => centers = value; }

    #region Unity Callbacks

    private void Start()
    {
        radialScale = detailLevel;
        //Build the HexSphere
        BuildWorld();
        //Set Tile colors, Generate Regions, Spawn gameobjects etc
        MapBuilder();
    }

    #endregion

    #region Public Methods

    public void SaveMeshes()
    {
        for (int index = 0; index < Tiles.Count; index++)
        {
            var tile = Tiles[index];
            Mesh mesh = new Mesh();
            mesh = tile.GetComponent<MeshFilter>().mesh;
            AssetDatabase.CreateAsset(mesh, $"Assets/Planet/Meshes/Iteration{detailLevel}/Tile{index}.asset");
            AssetDatabase.SaveAssets();
        }
    }

    #endregion

    #region Private Methods

    private void BuildWorld()
    {
        if (detailLevel < 1)
        {
            detailLevel = 1;
        }

        //Mesh generation freezes up for detail levels greater than 4
        if (detailLevel > 5)
        {
            detailLevel = 5;
        }

        radialScale = detailLevel;

        #region Generate Icosahedron Vertices

        //HEX VERTICES
        Geometry.Icosahedron(vectors, indices);
        //subdivide
        for (int i = 0; i < detailLevel; i++)
        {
            Geometry.Subdivide(vectors, indices, true);
        }

        // normalize vectors to "inflate" the icosahedron into a sphere.

        for (int i = 0; i < vectors.Count; i++)
        {
            vectors[i] = Vector3.Normalize(vectors[i]) * radialScale;
        }

        Centers = GetTriangleIncenter(vectors, indices);

        maxEdgeLength = GetMaxEdgeDistance(Centers);
        maxTileRadius = GetMaxTileRadius(Centers, vectors);

        GenerateSubMeshes(Centers, vectors);
        TileCount = Tiles.Count;

        worldRadius = Vector3.Magnitude(Centers[0]);

        #endregion
    }

    private void GenerateSubMeshes(List<Vector3> centers, List<Vector3> vertices)
    {
        //Generate the hex/pent mesh for each vertex in the main mesh by associating it to its surrounding triangle centers
        for (int i = 0; i < vertices.Count; i++)
        {
            GameObject tile = new GameObject($"Tile {i}");
            Mesh submesh = new Mesh();
            tile.AddComponent<MeshFilter>();
            //tile.transform.localPosition = vertices[i];
            List<Vector3> submeshVs = new List<Vector3>();
            for (int j = 0; j < centers.Count; j++)
            {
                if (Vector3.Distance(vertices[i], centers[j]) <= maxTileRadius)
                {
                    submeshVs.Add(centers[j]);
                }
            }

            //If its a pentagon
            if (submeshVs.Count == 5)
            {
                bool[] isUsed = new bool[5];
                List<int> orderedIndices = new List<int>();
                Vector3 current = submeshVs[0];
                orderedIndices.Add(0);
                isUsed[0] = true;
                //starting at the first point in submeshVs, find a point on the perimeter of the tile that is within one edge length from point current, then add its index to the list
                while (orderedIndices.Count < 5)
                {
                    foreach (Vector3 c in submeshVs)
                    {
                        if (Vector3.Distance(c, current) <= maxEdgeLength && Vector3.Distance(c, current) >= 0.001f && !isUsed[submeshVs.IndexOf(c)])
                        {
                            //triangles[h + j] = submeshVs.IndexOf(c);
                            orderedIndices.Add(submeshVs.IndexOf(c));
                            isUsed[submeshVs.IndexOf(c)] = true;
                            current = c;
                            break;
                        }
                    }
                }

                int[] triangles = new int[9];
                triangles[0] = 0;
                triangles[1] = orderedIndices[1];
                triangles[2] = orderedIndices[2];

                triangles[3] = orderedIndices[2];
                triangles[4] = orderedIndices[3];
                triangles[5] = orderedIndices[0];

                triangles[6] = orderedIndices[3];
                triangles[7] = orderedIndices[4];
                triangles[8] = orderedIndices[0];

                Vector3[] subVsArray = submeshVs.ToArray();
                submesh.vertices = subVsArray;
                submesh.triangles = triangles;
                Vector2[] uvs = new Vector2[submeshVs.Count];

                uvs[orderedIndices[0]] = new Vector2(0f, 0.625f);
                uvs[orderedIndices[1]] = new Vector2(0.5f, 1f);
                uvs[orderedIndices[2]] = new Vector2(1f, 0.625f);
                uvs[orderedIndices[3]] = new Vector2(0.8f, 0.0162f);
                uvs[orderedIndices[4]] = new Vector2(.1875f, 0.0162f);

                submesh.uv = uvs;
                tile.AddComponent<MeshRenderer>();
                tile.GetComponent<Renderer>().material = material;
            }
            //If its a hexagon
            else if (submeshVs.Count == 6)
            {
                bool[] isUsed = new bool[6];
                List<int> orderedIndices = new List<int>();
                Vector3 current = submeshVs[0];
                orderedIndices.Add(0);
                isUsed[0] = true;

                //starting at the first point in submeshVs, find a point on the perimeter of the tile that is within one edgelength from point current, then add its index to the list
                while (orderedIndices.Count < 6)
                {
                    foreach (Vector3 c in submeshVs)
                    {
                        if (Vector3.Distance(c, current) <= maxEdgeLength && Vector3.Distance(c, current) >= 0.001f && !isUsed[submeshVs.IndexOf(c)])
                        {
                            orderedIndices.Add(submeshVs.IndexOf(c));
                            isUsed[submeshVs.IndexOf(c)] = true;
                            current = c;
                            break;
                        }
                    }
                }

                int[] triangles = new int[12];
                triangles[0] = 0;
                triangles[1] = orderedIndices[1];
                triangles[2] = orderedIndices[2];

                triangles[3] = orderedIndices[2];
                triangles[4] = orderedIndices[3];
                triangles[5] = 0;

                triangles[6] = orderedIndices[3];
                triangles[7] = orderedIndices[4];
                triangles[8] = 0;

                triangles[9] = orderedIndices[4];
                triangles[10] = orderedIndices[5];
                triangles[11] = 0;
                Vector3[] subVsArray = submeshVs.ToArray();
                submesh.vertices = subVsArray;
                submesh.triangles = triangles;

                Vector2[] uvs = new Vector2[6];
                //UV Coords based on geometry of hexagon
                uvs[orderedIndices[0]] = new Vector2(0.0543f, 0.2702f);
                uvs[orderedIndices[1]] = new Vector2(0.0543f, 0.7272f);
                uvs[orderedIndices[2]] = new Vector2(0.5f, 1f);
                uvs[orderedIndices[3]] = new Vector2(0.946f, 0.7272f);
                uvs[orderedIndices[4]] = new Vector2(0.946f, 0.2702f);
                uvs[orderedIndices[5]] = new Vector2(0.5f, 0f);

                submesh.uv = uvs;
                tile.AddComponent<MeshRenderer>();

                //Single material
                tile.GetComponent<Renderer>().material = material;
            }

            //Assign mesh
            tile.GetComponent<MeshFilter>().mesh = submesh;
            submesh.RecalculateBounds();
            submesh.RecalculateNormals();
            tile.AddComponent<Tile>();

            //Fix any upsidedown tiles by checking their normal vector
            if (( submesh.normals[0] + vertices[i] ).magnitude < vertices[i].magnitude)
            {
                submesh.triangles = submesh.triangles.Reverse().ToArray();
                submesh.RecalculateBounds();
                submesh.RecalculateNormals();
            }

            //Initialize tile attributes
            tile.AddComponent<MeshCollider>();
            tile.transform.parent = transform;
            tile.isStatic = true;
            tile.GetComponent<Renderer>().material.color = Color.white;
            tile.tag = "Tile";
            Tiles.Add(tile);
        }
    }

    private void MapBuilder()
    {
        //Randomly assign colors
        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].GetComponent<Tile>().SetMaterial(material);
        }

        //find each tiles neighbors
        // for (int i = 0; i < Tiles.Count; i++)
        // {
        //     Tiles[i].GetComponent<Tile>().FindNeighbors();
        // }
    }

    private float GetMaxTileRadius(List<Vector3> centers, List<Vector3> vertices)
    {
        float delta = 1.5f;
        Vector3 v = Vector3.zero;
        if (detailLevel != 0)
        {
            v = vertices[12];
        }
        else
        {
            v = vertices[0];
        }

        float minDistance = Mathf.Infinity;
        foreach (Vector3 c in centers)
        {
            float dist = Vector3.Distance(v, c);

            if (dist < minDistance)
            {
                minDistance = dist;
            }
        }

        minDistance = minDistance * delta;

        return minDistance;
    }

    private float GetMaxEdgeDistance(List<Vector3> centers)
    {
        //Returns the approximate distance between adjacent triangle centers

        //delta is the approximate variation in edge lengths, as not all edges are the same length
        float delta = 1.4f;
        Vector3 point = centers[0];
        // scan all vertices to find nearest
        float minDistance = Mathf.Infinity;
        foreach (Vector3 n in centers)
        {
            if (!point.Equals(n))
            {
                float dist = Vector3.Distance(point, n);

                if (dist < minDistance)
                {
                    minDistance = dist;
                }
            }
        }

        minDistance = minDistance * delta;

        return minDistance;
    }

    private List<Vector3> GetTriangleIncenter(List<Vector3> vertices, List<int> triangles)
    {
        List<Vector3> centers = new List<Vector3>();
        for (int i = 0; i < triangles.Count - 2; i += 3)
        {
            Vector3 A = vertices[triangles[i]];
            Vector3 B = vertices[triangles[i + 1]];
            Vector3 C = vertices[triangles[i + 2]];

            float a = Vector3.Distance(C, B);
            float b = Vector3.Distance(A, C);
            float c = Vector3.Distance(A, B);

            float P = a + b + c;

            Vector3 abc = new Vector3(a, b, c);

            float x = Vector3.Dot(abc, new Vector3(A.x, B.x, C.x)) / P;
            float y = Vector3.Dot(abc, new Vector3(A.y, B.y, C.y)) / P;
            float z = Vector3.Dot(abc, new Vector3(A.z, B.z, C.z)) / P;

            Vector3 center = new Vector3(x, y, z);
            centers.Add(center);
        }

        return centers;
    }

    #endregion
}