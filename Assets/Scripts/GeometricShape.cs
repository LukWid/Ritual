using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometricShape : MonoBehaviour
{
    public void GenerateShape(string shapeName, Mesh mesh)
    {
       
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Constants.StandardShader);
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public virtual void GenerateMesh()
    {
        throw new NotImplementedException();
    }
}
