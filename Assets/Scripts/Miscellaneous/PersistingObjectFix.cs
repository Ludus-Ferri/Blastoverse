using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistingObjectFix : MonoBehaviour
{
    void LateUpdate()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
