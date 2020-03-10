using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAsteroidGenerator : MonoBehaviour
{
    public ObjectPooler asteroidPool;

    [Header("Asteroid Properties")]
    public int minimumVertices;
    public int maximumVertices;
    public float semiMajorAxis;
    public float semiMinorAxis;
    public float thetaDeviation;
    public float asteroidSize;

    public float spawnRate;
    float currentRate;

    void Start()
    {
        for (int i = 0; i < asteroidPool.objects.Count; i++)
        {
            asteroidPool.objects[i].GetComponent<BGAsteroid>().meshID = i;
        }

        semiMajorAxis *= asteroidSize;
        semiMinorAxis *= asteroidSize;

        currentRate = spawnRate;
    }

    void Update()
    {
        currentRate -= Time.deltaTime;
        if (currentRate < 0)
        {
            GenerateNewAsteroid();
        }
    }

    void GenerateNewAsteroid()
    {
        BGAsteroid asteroid;
        try
        {
            asteroid = asteroidPool.GetPooledObject().GetComponent<BGAsteroid>();
        }
        catch (System.NullReferenceException)
        {
            return;
        }

        MeshData data = CyclicPolygonGenerator.GeneratePolygon(AsteroidGenerationRNG.Instance.Next(minimumVertices, maximumVertices), semiMajorAxis * AsteroidGenerationRNG.Instance.NextFloat(0.8f, 1.25f), semiMinorAxis * AsteroidGenerationRNG.Instance.NextFloat(0.8f, 1.25f), thetaDeviation);

        asteroid.gameObject.SetActive(true);
        asteroid.SetMesh(data);
        asteroid.InitPosition();
        asteroid.SetColor(GameManager.Instance.backgroundColor);

        asteroid.InitialMove();

        asteroid.parentGenerator = this;

        currentRate = spawnRate;
    }
}
