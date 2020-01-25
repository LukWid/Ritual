using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    #region Serialized Fields

    [SerializeField]
    private Vector3 position;

    #endregion

    #region Public Properties

    public Vector3 Position { get => position; set {{ position = value; } }
}
    public List<Triangle> FiguresWithCommonVertex { get; set; }

    #endregion

    #region Constructors

    public Vertex(Vector3 position)
    {
        Position = position;
        FiguresWithCommonVertex = new List<Triangle>();
    }

    #endregion
}