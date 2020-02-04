using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    #region Serialized Fields

    [SerializeField]
    private Vector3 position;
    public int vertexIndex;

    #endregion

    #region Public Properties

    public Vector3 Position { get { return position; } set { position = value; } }
    public HashSet<Triangle> FiguresWithCommonVertex { get; set; }

    #endregion

    #region Constructors

    public Vertex(Vector3 position)
    {
        Position = position;
        FiguresWithCommonVertex = new HashSet<Triangle>();
    }
    public Vertex(Vector3 position, PlanetGenerator planetGenerator)
    {
        Position = position;
        vertexIndex = planetGenerator.GetVertexIndex(position);
        if (vertexIndex == -1)
        {
            vertexIndex = planetGenerator.vertices.Count;
        }

        FiguresWithCommonVertex = new HashSet<Triangle>();
    }

    public override bool Equals(object toCompare)
    {
        Vertex vertex = (Vertex) toCompare;
        if (vertex.Position == position)
        {
            return true;
        }

        return false;
    }

    #endregion

    public void Truncate()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        int count = 0;

        foreach (var figure in FiguresWithCommonVertex)
        {
            //List<Vertex> vertices = new List<Vertex>();
            //figure.GetVertices(vertices);
            foreach (var vertex in figure.Vertices)
            {
                if (!vertex.Equals(this))
                {
                    count++;
                    x += vertex.Position.x;
                    y += vertex.Position.y;
                    z += vertex.Position.z;
                }
            }
        }
        position.x = x / count;
        position.y = y / count;
        position.z = z / count;
    }
}