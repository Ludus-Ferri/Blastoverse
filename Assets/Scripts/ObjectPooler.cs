using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public int size = 100;

    public GameObject objectPrefab;

    public List<GameObject> objects;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        objects = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    /// <summary>
    /// Gets the first inactive object in the object pool.
    /// Returns null if no objects are available.
    /// </summary>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < size; i++)
        {
            if (!objects[i].activeInHierarchy)
                return objects[i];
        }

        return null;
    }
}
