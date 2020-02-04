using System;
using System.Collections.Generic;
using UnityEngine;

public class Vector3CoordComparer : IEqualityComparer<Vector3>
{
    #region Public Methods

    public bool Equals(Vector3 a, Vector3 b)
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

    public int GetHashCode(Vector3 obj)
    {
        //a cruder than default comparison, allows to compare very close-vector3's into same hash-code.
        return Math.Round(obj.x, 2).GetHashCode() ^ ( Math.Round(obj.y, 2).GetHashCode() << 2 ) ^ ( Math.Round(obj.z, 2).GetHashCode() >> 2 );
    }

    #endregion
}