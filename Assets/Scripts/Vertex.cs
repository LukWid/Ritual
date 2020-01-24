using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    [SerializeField]
    private Vector3 position;
    
    public Vector3 Position { get => position; set => position = value; }
    
    public Vertex(Vector3 position)
    {
        this.Position = position;
    }
}
