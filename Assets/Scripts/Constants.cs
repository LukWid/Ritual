using UnityEngine;

public static class Constants
{
    #region  Constants

    public static readonly double GoldenRatio = 1.61803398875;
    public static Shader StandardShader = Shader.Find("Standard");
    public static Material DefaultMaterial = new Material(StandardShader);

    #endregion

    #region Constructors

    static Constants()
    {
        DefaultMaterial.enableInstancing = true;
    }

    #endregion
}