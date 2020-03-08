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

    [Header("Asteroid Splitting")]
    public float areaThreshold;
    public float momentumFactor;

    [Header("Difficulty")]
    public float spawnRate;
    float currentRate;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < asteroidPool.objects.Count; i++) 
        {
            asteroidPool.objects[i].GetComponent<Asteroid>().meshID = i;
        }

        if (autoSize)
        {
            semiMajorAxis = maximumVertices / 10f;
            semiMinorAxis = minimumVertices / 10f;
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
        Asteroid asteroid = asteroidPool.GetPooledObject().GetComponent<Asteroid>();

        MeshData data = CyclicPolygonGenerator.GeneratePolygon(AsteroidGenerationRNG.Instance.Next(minimumVertices, maximumVertices), semiMajorAxis, semiMinorAxis, thetaDeviation);

        asteroid.SetMesh(data);
        asteroid.gameObject.SetActive(true);

        asteroid.InitialMove();

        asteroid.parentGenerator = this;

        currentRate = spawnRate;
    }

    public void GenerateAsteroidSlices(MeshData leftData, MeshData rightData, Vector2 leftCentroid, Vector2 rightCentroid, Vector3 preVelocity, Vector3 slicerVelocity)
    {
        Asteroid leftAsteroid, rightAsteroid;
        if (leftData.GetArea() > areaThreshold)
        {
            leftAsteroid = asteroidPool.ActivateObject().GetComponent<Asteroid>();
            leftAsteroid.SetMesh(leftData);
            leftAsteroid.transform.position = leftCentroid;
            leftAsteroid.Move((preVelocity + slicerVelocity - Vector3.right) * momentumFactor);
        }

        if (rightData.GetArea() > areaThreshold)
        {
            rightAsteroid = asteroidPool.ActivateObject().GetComponent<Asteroid>();
            rightAsteroid.SetMesh(rightData);
            rightAsteroid.transform.position = rightCentroid;
            rightAsteroid.Move((preVelocity + slicerVelocity + Vector3.right) * momentumFactor);
        }

        

    }
}
