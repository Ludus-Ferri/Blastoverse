using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicPolygonGenerator
{

    private static Vector2 GetPointOnEllipse(float semiMajorAxis, float semiMinorAxis, float theta)
    {
        return new Vector2(semiMajorAxis * Mathf.Cos(theta * Mathf.Deg2Rad), semiMinorAxis * Mathf.Sin(theta * Mathf.Deg2Rad));
    }

    private static List<int> TriangulateConvexPolygon(int n)
    {
        List<int> tris = new List<int>();

        for (int i = 0; i < n - 2; i++)
        {
            tris.Add(0);
            tris.Add(i + 1);
            tris.Add(i + 2);
        }

        return tris;
    }

    public static MeshData GeneratePolygon(int n, float semiMajorAxis, float semiMinorAxis, float thetaDeviation)
    {
        List<Vector3> verts = new List<Vector3>();

        if (n < 3)
        {
            Debug.LogError("Attempted to generate a polygon with n < 3.");
            return new MeshData();
        }

        if (thetaDeviation * 2 >= 360f)
        {
            Debug.LogWarning("Tried to generate a polygon with thetaDeviation too high. This will cause vertices to go out of order, and the triangulation might fail.");
        }

        // Generate angles
        List<float> angles = new List<float>(n);
        for (int i = 0; i < n; i++)
            angles.Add(360f / n * i + AsteroidGenerationRNG.Instance.NextFloat(-thetaDeviation / n, thetaDeviation / n));

        foreach (float a in angles)
        {
            Vector2 point = GetPointOnEllipse(semiMajorAxis, semiMinorAxis, a);
            verts.Add(point);
        }

        MeshData meshData = new MeshData
        {
            vertices = verts.ToArray(),
            triangles = TriangulateConvexPolygon(n).ToArray()
        };

        return meshData;
    }
}
