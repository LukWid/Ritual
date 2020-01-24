using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : GeometricShape
{
    private Vertex a;
    private Vertex b;
    private Vertex c;
    private Vertex d;

    public Vertex A { get => a; set => a = value; }
    public Vertex B { get => b; set => b = value; }
    public Vertex C { get => c; set => c = value; }
    public Vertex D { get => d; set => d = value; }

    public Quad(Vertex a, Vertex b, Vertex c, Vertex d)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
    }

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
}
