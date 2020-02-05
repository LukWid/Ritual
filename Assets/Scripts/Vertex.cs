using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    #region Serialized Fields

    private Vector3 position;
    public int vertexIndex;

    #endregion

    #region Public Properties

    public Vector3 Position { get { return position; } set { position = value; } }

    #endregion

    #region Constructors

    public Vertex(Vector3 position, PlanetGenerator planetGenerator)
    {
        Position = position;
        vertexIndex = planetGenerator.GetVertexIndex(position);
        if (vertexIndex == -1)
        {
            vertexIndex = planetGenerator.Vertices.Count;
        }
    }

    #endregion
}