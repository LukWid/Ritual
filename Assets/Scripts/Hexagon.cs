using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    #region Private Fields

    private List<Triangle> triangles;
    private Vertex middle;

    #endregion

    #region Constructors

    public Hexagon(Vertex middle, Triangle a, Triangle b, Triangle c, Triangle d, Triangle e, Triangle f)
    {
        triangles = new List<Triangle>();
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
        triangles.Add(d);
        triangles.Add(e);
        triangles.Add(f);

        this.middle = middle;
    }

    #endregion

    #region Public Methods

    public void TruncateHex()
    {
        middle.Truncate();
    }

    #endregion
}