using System;
using UnityEngine;

public class GeometricShape : MonoBehaviour
{
    #region Public Methods

    public void GenerateShape(string shapeName, Mesh mesh)
    {
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = Constants.DefaultMaterial;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public virtual void GenerateMesh()
    {
        throw new NotImplementedException();
    }

    #endregion
}