using System;
using UnityEngine;
using UnityEngine.Rendering;

public class GeometricShape : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private GameObject gameObject;
        
    #region Public Methods

    public void GenerateShape(string shapeName, Mesh mesh, GameObject gameObject)
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
        meshRenderer.sharedMaterial = Constants.DefaultMaterial;
        
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        this.gameObject = gameObject;
    }

    protected void RecalculateShape(Mesh mesh)
    {
        meshFilter.mesh = mesh;
    }

    public virtual void GenerateMesh(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    #endregion
}