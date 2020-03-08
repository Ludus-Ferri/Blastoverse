using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BGAsteroid : MonoBehaviour
{
    [HideInInspector]
    public int meshID;

    public float spawnDistance;
    public float initialMoveSpeed, initialMoveRotation;
    public float moveDeviationScale;
    public float baseDepth;

    public float colliderSizeFactor;

    Rigidbody2D rb2D;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    EdgeCollider2D col;

    float depth;

    [HideInInspector]
    public BGAsteroidGenerator parentGenerator;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<EdgeCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetMesh(MeshData data)
    {
        Mesh mesh = MeshStorage.Instance.GetBGAsteroidMeshFor(meshID);

        mesh.Clear();
        mesh.SetVertices(data.vertices);
        mesh.SetTriangles(data.triangles, 0);
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        Vector2[] colPath = new Vector2[mesh.vertexCount + 1];
        for (int i = 0; i < mesh.vertexCount; i++)
            colPath[i] = new Vector2(mesh.vertices[i].x * colliderSizeFactor, mesh.vertices[i].y * colliderSizeFactor);

        colPath[mesh.vertexCount] = colPath[0];

        col.points = colPath;
    }

    public void SetColor(Color color)
    {
        depth = baseDepth + AsteroidGenerationRNG.Instance.NextFloat(-1, 1);
        meshRenderer.material.SetColor("_BaseColor", Color.Lerp(Color.white, color, depth / baseDepth - 0.15f));

    }

    public void InitialMove()
    {
        Vector2 ran = UnityEngine.Random.insideUnitCircle.normalized;
        Vector2 V = UnityEngine.Random.insideUnitCircle.normalized;
        transform.position = new Vector3(V.x * spawnDistance, V.y * spawnDistance, depth);
        Vector3 forceToAdd = new Vector3(-transform.position.x + ran.x * moveDeviationScale, -transform.position.y + ran.y * moveDeviationScale, 0);
        rb2D.AddForce(forceToAdd * initialMoveSpeed);

        rb2D.AddTorque(initialMoveRotation);
    }
}
