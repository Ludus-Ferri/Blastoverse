using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public int meshID;

    MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void SetMesh()
    {
        meshFilter.mesh = MeshStorage.Instance.GetAsteroidMeshFor(meshID);
    }
}
