using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStorage : MonoBehaviour
{
    public static MeshStorage Instance;

    public Dictionary<int, Mesh> asteroidMeshes;
    public Dictionary<int, Mesh> bgAsteroidMeshes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        asteroidMeshes = new Dictionary<int, Mesh>();
        bgAsteroidMeshes = new Dictionary<int, Mesh>();
    }

    public Mesh GetAsteroidMeshFor(int id)
    {
        if (asteroidMeshes.ContainsKey(id)) return asteroidMeshes[id];
        else asteroidMeshes.Add(id, new Mesh());
        return asteroidMeshes[id];
    }

    public Mesh GetBGAsteroidMeshFor(int id)
    {
        if (bgAsteroidMeshes.ContainsKey(id)) return bgAsteroidMeshes[id];
        else bgAsteroidMeshes.Add(id, new Mesh());
        return bgAsteroidMeshes[id];
    }
}
