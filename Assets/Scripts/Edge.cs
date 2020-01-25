using UnityEngine;

public class Edge
{
    #region Private Fields

    private Vertex a;
    private Vertex b;

    #endregion

    #region Public Properties

    public Vertex A { get => a; set => a = value; }
    public Vertex B { get => b; set => b = value; }

    #endregion

    #region Constructors

    public Edge(Vertex a, Vertex b)
    {
        A = a;
        B = b;
    }

    #endregion

    #region Public Methods

    public Vertex[] DivideEdge(int frequency)
    {
        Vertex[] dividedVertexArray = new Vertex[frequency + 1];

        dividedVertexArray[0] = A;
        dividedVertexArray[frequency] = B;

        for (int i = 1; i < frequency; i++)
        {
            Vertex newVertex = new Vertex(Vector3.Lerp(A.Position, B.Position, 1.0f / frequency * i));
            dividedVertexArray[i] = newVertex;

            //GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //indicator.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            //indicator.transform.position = newVertex.Position;
        }

        return dividedVertexArray;
    }

    #endregion
}