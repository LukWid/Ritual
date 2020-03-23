using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Geometry
{
    #region Public Methods

    /// <remarks>
    ///     i0
    ///     /  \
    ///     m02-m01
    ///     /  \ /  \
    ///     i2---m12---i1
    /// </remarks>
    /// <param name="vectors"></param>
    /// <param name="indices"></param>
    public static void Subdivide(List<Vector3> vectors, List<int> indices, bool removeSourceTriangles)
    {
        var midpointIndices = new Dictionary<string, int>();

        var newIndices = new List<int>(indices.Count * 4);

        if (!removeSourceTriangles)
        {
            newIndices.AddRange(indices);
        }

        for (var i = 0; i < indices.Count - 2; i += 3)
        {
            var i0 = indices[i];
            var i1 = indices[i + 1];
            var i2 = indices[i + 2];

            var m01 = GetMidpointIndex(midpointIndices, vectors, i0, i1);
            var m12 = GetMidpointIndex(midpointIndices, vectors, i1, i2);
            var m02 = GetMidpointIndex(midpointIndices, vectors, i2, i0);

            newIndices.AddRange(new[] { i0, m01, m02, i1, m12, m01, i2, m02, m12, m02, m01, m12 });
        }

        indices.Clear();
        indices.AddRange(newIndices);
    }

    /// <summary>
    ///     create a regular icosahedron (20-sided polyhedron)
    /// </summary>
    /// <param name="primitiveType"></param>
    /// <param name="size"></param>
    /// <param name="vertices"></param>
    /// <param name="indices"></param>
    /// <remarks>
    ///     You can create this programmatically instead of using the given vertex
    ///     and index list, but it's kind of a pain and rather pointless beyond a
    ///     learning exercise.
    /// </remarks>
    public static void Icosahedron(List<Vector3> vertices, List<int> indices)
    {
        indices.AddRange(new[]
        {
            4, 0, 5,
            4, 3, 0,
            3, 1, 0,
            0, 1, 2,
            0, 2, 5,
            2, 8, 5,
            2, 7, 8,
            1, 7, 2,
            1, 6, 7,
            6, 11, 7,
            11, 8, 7,
            11, 10, 8,
            11, 9, 10,
            9, 4, 10,
            4, 5, 10,
            10, 5, 8,
            3, 4, 9,
            3, 9, 6,
            3, 6, 1,
            11, 6, 9
        }.Select(i => i + vertices.Count));

        var X = 0.525731112119133606f;
        var Z = 0.850650808352039932f;

        vertices.AddRange(new[]
        {
            new Vector3(0f, Z, X), //0	//4
            new Vector3(0f, Z, -X), //1	//5
            new Vector3(Z, X, 0f), //2	//8
            new Vector3(-Z, X, 0f), //3	//9
            new Vector3(-X, 0f, Z), //4	//0
            new Vector3(X, 0f, Z), //5	//1
            new Vector3(-X, 0f, -Z), //6	//2
            new Vector3(X, 0f, -Z), //7	//3
            new Vector3(Z, -X, 0f), //8	//10
            new Vector3(-Z, -X, 0f), //9	//11
            new Vector3(0f, -Z, X), //10//6
            new Vector3(0f, -Z, -X) //11//7
        });
    }

    #endregion

    #region Private Methods

    private static int GetMidpointIndex(Dictionary<string, int> midpointIndices, List<Vector3> vertices, int i0, int i1)
    {
        var edgeKey = string.Format("{0}_{1}", Mathf.Min(i0, i1), Mathf.Max(i0, i1));

        var midpointIndex = -1;

        if (!midpointIndices.TryGetValue(edgeKey, out midpointIndex))
        {
            var v0 = vertices[i0];
            var v1 = vertices[i1];

            var midpoint = ( v0 + v1 ) / 2f;

            int foundIndex = -1;

            if (ListContains(vertices, midpoint, out foundIndex))
            {
                midpointIndex = foundIndex;
            }
            else
            {
                midpointIndex = vertices.Count;
                vertices.Add(midpoint);
            }
        }

        return midpointIndex;
    }

    private static bool ListContains(List<Vector3> vertices, Vector3 toFind, out int index)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            var vertex = vertices[i];
            if (VertexEquals(vertex, toFind))
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

    private static bool VertexEquals(Vector3 a, Vector3 b)
    {
        if (Mathf.Abs(a.x - b.x) > 0.01)
        {
            return false;
        }

        if (Mathf.Abs(a.y - b.y) > 0.01)
        {
            return false;
        }

        if (Mathf.Abs(a.z - b.z) > 0.01)
        {
            return false;
        }

        return true; //indeed, very close
    }

    #endregion
}