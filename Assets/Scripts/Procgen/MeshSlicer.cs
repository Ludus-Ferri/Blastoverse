using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MeshSlicer
{
    static Vector3 GetCentroid(List<Vector3> points)
    {
        float sumX = points.Sum(v => v.x), sumY = points.Sum(v => v.y);
        return new Vector3(sumX, sumY) / points.Count;
    }

    static float GetLineSideOfPoint(Vector2 lineP1, Vector2 lineP2, Vector2 point)
    {
        return (point.x - lineP1.x) * (lineP2.y - lineP1.y) - (point.y - lineP1.y) * (lineP2.x - lineP1.x);
    }

    /// <summary>
    /// Slices a mesh given by data into two parts, along a line that intersects lineP1 and lineP2.
    /// </summary>
    /// <param name="data">MeshData of the mesh to slice.</param>
    /// <param name="raycastDir">Direction along which to raycast from the hit point.</param>
    /// <param name="hitPoint">First point that defines the slice line.</param>
    /// <param name="meshPos">Position of the mesh to cut.</param>
    /// <returns>Two MeshDatas of sliced meshes. Index 0 corresponds to the left-side slice, index 1 to the right-side slice.</returns>
    public static MeshData[] Slice(MeshData data, Vector2 raycastDir, Vector2 hitPoint, Vector2 meshPos)
    {
        Vector2[] verts = new Vector2[data.vertices.Length];
        for (int j = 0; j < data.vertices.Length; j++) verts[j] = new Vector2(data.vertices[j].x, data.vertices[j].y);

        // Find hit points
        Ray ray = new Ray(hitPoint + raycastDir * 0.1f, raycastDir);
        RaycastHit2D otherHit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, LayerMask.GetMask("Asteroid"));
        Vector2 slicePoint = otherHit.point - meshPos;

        RaycastHit2D firstHit = Physics2D.Raycast(hitPoint, ray.direction, 10f, LayerMask.GetMask("Asteroid"));

        hitPoint = firstHit.point - meshPos;

        MeshData positiveSliceData = new MeshData(), negativeSliceData = new MeshData();

        // Can be optimized

        List<Vector3> positiveVertices = new List<Vector3>(), negativeVertices = new List<Vector3>();

        foreach (Vector2 v in data.vertices) 
        { 
            float pointSide = GetLineSideOfPoint(hitPoint, slicePoint, v);

            if (pointSide > 0) positiveVertices.Add(v);
            if (pointSide < 0) negativeVertices.Add(v);
        }

        positiveVertices.Add(hitPoint);
        positiveVertices.Add(slicePoint);
        negativeVertices.Add(hitPoint);
        negativeVertices.Add(slicePoint);

        Vector3 posCentroid = GetCentroid(positiveVertices);
        Vector3 negCentroid = GetCentroid(negativeVertices);

        positiveVertices = positiveVertices.OrderBy(v => Mathf.Atan2(v.y - posCentroid.y, v.x - posCentroid.x)).ToList();
        negativeVertices = negativeVertices.OrderBy(v => Mathf.Atan2(v.y - negCentroid.y, v.x - negCentroid.x)).ToList();

        positiveSliceData.triangles = CyclicPolygonGenerator.TriangulateConvexPolygon(positiveVertices.Count).ToArray();
        negativeSliceData.triangles = CyclicPolygonGenerator.TriangulateConvexPolygon(negativeVertices.Count).ToArray();

        positiveSliceData.vertices = positiveVertices.ToArray();
        negativeSliceData.vertices = negativeVertices.ToArray();

        return new MeshData[2] { negativeSliceData, positiveSliceData };
    }
}
