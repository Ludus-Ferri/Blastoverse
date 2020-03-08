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
}
