using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangle
{
    #region Private Fields

    private Vertex a;
    private Vertex b;
    private Vertex c;

    private int targetSubdivision;
    private int actualSubdivision;
    
    public List<Triangle> dividedTriangles;
    public List<Triangle> neighbours;
    
    public List<Triangle> Neighbours { get => neighbours; set => neighbours = value; }
    public List<Vertex> Vertices {get => new List<Vertex>(){a,b,c};}

    public int v1;
    public int v2;
    public int v3;

    public int trisIndex;
    
    private PlanetGenerator planetGenerator;
    
    #endregion

    #region Public Methods

    public Triangle(int v1, int v2, int v3, List<Triangle> allTriangles, PlanetGenerator planetGenerator)
    {
        this.v1 = v1; 
        this.v2 = v2; 
        this.v3 = v3;

        trisIndex = allTriangles.Count;
        
        AddToDictionary(v1, planetGenerator.VertexTriangles, planetGenerator.Vertices);
        AddToDictionary(v2, planetGenerator.VertexTriangles, planetGenerator.Vertices);
        AddToDictionary(v3, planetGenerator.VertexTriangles, planetGenerator.Vertices);
    }

    private void AddToDictionary(int index, Dictionary<Vector3, List<Triangle>> vertexTriangles, List<Vertex> vertices)
    {
        if (vertexTriangles.ContainsKey(vertices[index].Position))
        {
            vertexTriangles[vertices[index].Position].Add(this);
        }
        else
        {
            vertexTriangles[vertices[index].Position] = new List<Triangle>();
            vertexTriangles[vertices[index].Position].Add(this);
        }
    }
    
    public int[] GetVertices()
    {
        return new int[3]{ v1,v2,v3};
    }
    
    #endregion
}