using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MeshSlicer
{
    /// <summary>
    /// Slices a mesh given by data into two parts, along a line that intersects lineP1 and lineP2.
    /// </summary>
    /// <param name="data">MeshData of the mesh to slice.</param>
    /// <param name="lineP1">First point that defines the slice line.</param>
    /// <param name="lineP2">Second point that defines the slice line.</param>
    /// <returns>Two MeshDatas of sliced meshes. Index 0 corresponds to the left-side slice, index 1 to the right-side slice.</returns>
    public static MeshData[] Slice(MeshData data, Vector2 bulletDirection, Vector2 hitPoint)
    {
        Vector2[] verts = new Vector2[data.vertices.Length];
        for (int i = 0; i < data.vertices.Length; i++) verts[i] = new Vector2(data.vertices[i].x, data.vertices[i].y);

        // Find second hit point
        Ray ray = new Ray(hitPoint + bulletDirection * 0.1f, bulletDirection);
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 10f);

        Debug.Log(Physics2D.Raycast(Vector2.zero, Vector2.up, 1000f, LayerMask.GetMask("Asteroid")).point);
        
        RaycastHit2D otherHit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, LayerMask.GetMask("Asteroid"));

        Debug.Log(otherHit.point);
        Debug.DrawLine(hitPoint, otherHit.point, Color.magenta, 10f);

        // TODO: Divide the mesh along the line formed by hitPoint and otherHit.point

        return null;
    }
}
