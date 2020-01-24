using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad
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

    public void GenerateMesh(ref List<Mesh> meshes)
    {
        meshes.Add(new Triangle(A, B, C).GenerateMesh()); 
        meshes.Add(new Triangle(A, C, D).GenerateMesh());
    }
}
