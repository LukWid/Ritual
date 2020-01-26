using System.Collections.Generic;
using UnityEngine;

public class Edge : GeometricShape
{
    #region Private Fields

    private Vertex a;
    private Vertex b;

    #endregion

    #region Public Properties

    public Vertex A { get => a; set => a = value; }
    public Vertex B { get => b; set => b = value; }
    public List<Triangle> FiguresWithCommonEdge { get; set; }
    public bool Divided { get; set; }

    #endregion

    #region Constructors

    public Edge(Vertex a, Vertex b)
    {
        A = a;
        B = b;
        FiguresWithCommonEdge = new List<Triangle>();
    }

    #endregion

    #region Public Methods

    public Vertex[] DivideEdge(int frequency, bool drawIndicator = true)
    {
        if (!Divided)
        {
            Vertex[] dividedVertexArray = new Vertex[frequency + 1];

            dividedVertexArray[0] = A;
            dividedVertexArray[frequency] = B;

            for (int i = 1; i < frequency; i++)
            {
                Vertex newVertex = new Vertex(Vector3.Lerp(A.Position, B.Position, 1.0f / frequency * i));
                dividedVertexArray[i] = newVertex;

                if (drawIndicator)
                {
                    GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    indicator.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    indicator.transform.position = newVertex.Position;
                }
            }

            Divided = true;
            return dividedVertexArray;
        }

        return null;
    }

    public override bool Equals(object toCompare)
    {
        Edge compare = (Edge) toCompare;
        if ((a.Position == compare.A.Position && b.Position == compare.B.Position) || (b.Position == compare.A.Position && a.Position == compare.B.Position))
        {
            return true;
        }

        return false;
    }

    #endregion
}