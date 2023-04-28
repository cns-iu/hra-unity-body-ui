using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixExtensions
{
    /// <summary>
    /// Utility func that builds a matrix based off a string of floats
    /// </summary>
    /// <param name="transformMatrix"></param>
    /// <returns></returns>
    public static Matrix4x4 BuildMatrix(float[] transformMatrix)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetColumn(0, new Vector4(transformMatrix[0], transformMatrix[1], transformMatrix[2], transformMatrix[3]));
        matrix.SetColumn(1, new Vector4(transformMatrix[4], transformMatrix[5], transformMatrix[6], transformMatrix[7]));
        matrix.SetColumn(2, new Vector4(transformMatrix[8], transformMatrix[9], transformMatrix[10], transformMatrix[11]));
        matrix.SetColumn(3, new Vector4(transformMatrix[12], transformMatrix[13], transformMatrix[14], transformMatrix[15]));

        return matrix;
    }

    /// <summary>
    /// Utility function to flip the matrix along the z axis
    /// </summary>
    /// <returns></returns>
    public static Matrix4x4 ReflectZ()
    {
        var result = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, -1, 0),
            new Vector4(0, 0, 0, 1)
        );
        return result;
    }
}