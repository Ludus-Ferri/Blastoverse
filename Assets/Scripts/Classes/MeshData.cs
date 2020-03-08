using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeshData
{
    public Vector3[] vertices;
    public int[] triangles;

    public float GetArea()
    {
        float area = 0;
        int j = vertices.Length - 1;

        for (int i = 0; i < vertices.Length; i++)
        {
            area += (vertices[j].x + vertices[i].x) * (vertices[j].y - vertices[i].y);
            j = i;
        }

        return Mathf.Abs(area / 2);
    }

    // Rotates vertices about the origin (0, 0)
    public Vector3[] GetRotatedVertices(float rotation)
    {
        Vector3[] result = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            result[i] = new Vector3(vertices[i].x * Mathf.Cos(rotation * Mathf.Deg2Rad) - vertices[i].y * Mathf.Sin(rotation * Mathf.Deg2Rad),
                vertices[i].x * Mathf.Sin(rotation * Mathf.Deg2Rad) + vertices[i].y * Mathf.Cos(rotation * Mathf.Deg2Rad));
        return result;
    }
}
