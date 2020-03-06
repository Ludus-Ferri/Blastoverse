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
}
