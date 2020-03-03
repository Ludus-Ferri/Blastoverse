using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStorage : MonoBehaviour
{
    public static MeshStorage Instance;

    public Dictionary<int, Mesh> asteroidMeshes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        asteroidMeshes = new Dictionary<int, Mesh>();
    }

    public Mesh GetAsteroidMeshFor(int id)
    {
        if (asteroidMeshes.ContainsKey(id)) return asteroidMeshes[id];
        else asteroidMeshes.Add(id, new Mesh());
        return asteroidMeshes[id];
    }

    public void SetAsteroidMeshFor(int id, Vector3[] verts, int[] tris)
    {
        if (!asteroidMeshes.ContainsKey(id))
            asteroidMeshes[id] = new Mesh();

        asteroidMeshes[id].SetVertices(verts);
        asteroidMeshes[id].SetTriangles(tris, 0);
        asteroidMeshes[id].RecalculateNormals();
    }
}
