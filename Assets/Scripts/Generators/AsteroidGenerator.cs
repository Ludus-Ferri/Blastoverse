using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public ObjectPooler asteroidPool;

    [Header("Asteroid Properties")]
    public int minimumVertices;
    public int maximumVertices;
    public float semiMajorAxis;
    public float semiMinorAxis;
    public float thetaDeviation;

    [Header("Auto Size")]
    public bool autoSize;
    public float asteroidSize;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < asteroidPool.objects.Count; i++) 
        {
            asteroidPool.objects[i].GetComponent<Asteroid>().meshID = i;
        }

        if (autoSize)
        {
            semiMajorAxis = maximumVertices * asteroidSize / 50f;
            semiMinorAxis = minimumVertices * asteroidSize / 50f;
        }

        // FOR DEBUG PURPOSES ONLY
        GenerateNewAsteroid();
    }

    void GenerateNewAsteroid() 
    {
        Asteroid asteroid = asteroidPool.GetPooledObject().GetComponent<Asteroid>();

        MeshData data = CyclicPolygonGenerator.GeneratePolygon(AsteroidGenerationRNG.Instance.Next(minimumVertices, maximumVertices), semiMajorAxis, semiMinorAxis, thetaDeviation);

        MeshStorage.Instance.SetAsteroidMeshFor(asteroid.meshID, data.vertices, data.triangles);
        asteroid.SetMesh();
        asteroid.gameObject.SetActive(true);
    }
}
