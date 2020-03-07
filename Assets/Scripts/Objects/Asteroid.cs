using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public int meshID;

    public float spawnDistance;
    public float initialMoveSpeed;
    public float moveDeviationScale;

    Rigidbody2D rb2D;

    MeshData meshData;

    MeshFilter meshFilter;
    EdgeCollider2D col;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponent<EdgeCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetMesh(MeshData data)
    {
        Mesh mesh = MeshStorage.Instance.GetAsteroidMeshFor(meshID);

        mesh.Clear();
        mesh.SetVertices(data.vertices);
        mesh.SetTriangles(data.triangles, 0);
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshData = data;

        Vector2[] colPath = new Vector2[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++)
            colPath[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

        col.points = colPath;
    }

    public void InitialMove()
    {
        Vector2 ran = Random.insideUnitCircle.normalized; 
        Vector2 V = Random.insideUnitCircle.normalized;
        transform.position = new Vector3(V.x*spawnDistance, V.y*spawnDistance, 0);
        Vector3 forceToAdd = new Vector3(-transform.position.x + ran.x * moveDeviationScale, -transform.position.y + ran.y * moveDeviationScale, 0);
        rb2D.AddForce((forceToAdd) * initialMoveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            MeshSlicer.Slice(meshData, collision.rigidbody.velocity, collision.GetContact(0).point);
        }
    }
}
