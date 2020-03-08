using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public int size = 100;

    public GameObject objectPrefab;

    public List<GameObject> objects;

    private void Awake()
    {        
        objects = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            obj.tag = "Poolable";
            obj.transform.parent = transform;

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

    public GameObject ActivateObject()
    {
        GameObject obj = GetPooledObject();
        obj.SetActive(true);
        return obj;
    }
}
