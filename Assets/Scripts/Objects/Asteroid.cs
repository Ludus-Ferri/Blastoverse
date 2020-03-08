using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public int meshID;

    public float spawnDistance;
    public float initialMoveSpeed, initialMoveRotation;
    public float moveDeviationScale;

    public float colliderSizeFactor;

    Rigidbody2D rb2D;

    MeshData meshData;

    MeshFilter meshFilter;
    EdgeCollider2D col;

    [HideInInspector]
    public AsteroidGenerator parentGenerator;

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

        Vector2[] colPath = new Vector2[mesh.vertexCount + 1];
        for (int i = 0; i < mesh.vertexCount; i++)
            colPath[i] = new Vector2(mesh.vertices[i].x * colliderSizeFactor, mesh.vertices[i].y * colliderSizeFactor);

        colPath[mesh.vertexCount] = colPath[0];

        col.points = colPath;
    }

    public void InitialMove()
    {
        Vector2 ran = Random.insideUnitCircle.normalized; 
        Vector2 V = Random.insideUnitCircle.normalized;
        transform.position = new Vector3(V.x*spawnDistance, V.y*spawnDistance, 0);
        Vector3 forceToAdd = new Vector3(-transform.position.x + ran.x * moveDeviationScale, -transform.position.y + ran.y * moveDeviationScale, 0);
        rb2D.AddForce(forceToAdd * initialMoveSpeed);

        rb2D.AddTorque(initialMoveRotation);
    }

    public void Move(Vector3 velocity, float torque)
    {
        rb2D.AddForce(velocity);
        rb2D.AddTorque(torque);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            float area = meshData.GetArea();

            GameManager.Instance.mainCamera.GetComponent<CameraShake>().InduceMotion(area * (transform.position - GameManager.Instance.playerController.transform.position).magnitude / 10f);

            if (area < parentGenerator.areaThreshold)
                gameObject.SetActive(false);
            else
            {
                Vector3 vel = collider.GetComponent<Rigidbody2D>().velocity;

                MeshData[] slices = MeshSlicer.Slice(meshData, vel, collider.transform.position, transform.position, transform.rotation, out Vector2 posCentroid, out Vector2 negCentroid);
                collider.gameObject.SetActive(false);

                if (slices == null) return;

                gameObject.SetActive(false);

                parentGenerator.GenerateAsteroidSlices(slices[0], slices[1], negCentroid, posCentroid, rb2D.velocity, vel);

                
            }

            
        }
    }
}
