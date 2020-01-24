using System.Collections.Generic;
using UnityEngine;

public class Quad : GeometricShape
{
    #region Private Fields

    private Vertex a;
    private Vertex b;
    private Vertex c;
    private Vertex d;

    #endregion

    #region Public Properties

    public List<Vertex> Vertices => new List<Vertex> { a, b, c, d };

    public Vertex A { get => a; set => a = value; }
    public Vertex B { get => b; set => b = value; }
    public Vertex C { get => c; set => c = value; }
    public Vertex D { get => d; set => d = value; }

    #endregion

    #region Constructors

    public Quad(Vertex a, Vertex b, Vertex c, Vertex d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    #endregion

    #region Public Methods

    public override void GenerateMesh()
    {
        GameObject Triangle1 = new GameObject("Triangle1");
        GameObject Triangle2 = new GameObject("Triangle2");

        Triangle triangle1Script = Triangle1.AddComponent<Triangle>();
        triangle1Script.Initialize(A, B, C);
        triangle1Script.GenerateMesh();

        Triangle triangle2Script = Triangle2.AddComponent<Triangle>();
        triangle2Script.Initialize(A, C, D);
        triangle2Script.GenerateMesh();
    }

    #endregion
}