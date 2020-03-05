using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public int meshID;

    MeshFilter meshFilter;
    PolygonCollider2D col;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponent<PolygonCollider2D>();
    }

    public void SetMesh()
    {
        Mesh mesh = MeshStorage.Instance.GetAsteroidMeshFor(meshID);

        meshFilter.mesh = mesh;

        Vector2[] colPath = new Vector2[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++)
            colPath[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

        col.SetPath(0, colPath);
    }
}
