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
    /// <param name="negCentroid">Position of the negative mesh's centroid.</param>
    /// <param name="posCentroid">Position of the positive mesh's centroid.</param>
    /// <returns>Two MeshDatas of sliced meshes. Index 0 corresponds to the left-side slice, index 1 to the right-side slice.</returns>
    public static MeshData[] Slice(MeshData data, Vector2 raycastDir, Vector2 hitPoint, Vector2 meshPos, out Vector2 posCentroid, out Vector2 negCentroid)
    {
        // Find hit points
        Ray ray = new Ray(hitPoint, raycastDir);
        RaycastHit2D firstHit = Physics2D.Raycast(ray.origin, ray.direction, 2f, LayerMask.GetMask("Asteroid"));
        RaycastHit2D otherHit = Physics2D.Raycast(new Vector3(firstHit.point.x, firstHit.point.y) + ray.direction * 0.05f, ray.direction, 2f, LayerMask.GetMask("Asteroid"));
        hitPoint = firstHit.point - meshPos;

        if (firstHit.collider == null)
        {
            Debug.LogError("No firstHit collider found!");

            posCentroid = Vector2.zero;
            negCentroid = Vector2.zero;
            return null;
        }

        if (otherHit.collider == null)
        {
            Debug.LogError("No slicePoint collider found!");
            Debug.Log($"First hit point: {firstHit.point.x} {firstHit.point.y}");

            posCentroid = Vector2.zero;
            negCentroid = Vector2.zero;
            return null;
        }
        
        Vector2 slicePoint = otherHit.point - meshPos;
        Debug.DrawLine(hitPoint + meshPos, slicePoint + meshPos, Color.black, 10);
        Debug.Log(hitPoint + meshPos);
        Debug.Log(slicePoint + meshPos);

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

        posCentroid = GetCentroid(positiveVertices);
        negCentroid = GetCentroid(negativeVertices);

        Vector2 pc = posCentroid, nc = negCentroid;

        positiveVertices = positiveVertices.OrderBy(v => Mathf.Atan2(v.y - pc.y, v.x - pc.x)).ToList();
        negativeVertices = negativeVertices.OrderBy(v => Mathf.Atan2(v.y - nc.y, v.x - nc.x)).ToList();

        posCentroid += meshPos;
        negCentroid += meshPos;

        positiveSliceData.triangles = CyclicPolygonGenerator.TriangulateConvexPolygon(positiveVertices.Count).ToArray();
        negativeSliceData.triangles = CyclicPolygonGenerator.TriangulateConvexPolygon(negativeVertices.Count).ToArray();

        positiveSliceData.vertices = positiveVertices.ToArray();
        negativeSliceData.vertices = negativeVertices.ToArray();


        return new MeshData[2] { negativeSliceData, positiveSliceData };
    }
}
