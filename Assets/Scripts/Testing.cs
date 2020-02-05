using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Testing testing = new Testing();

        testing.GetNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int GetNumber()
    {
        return 3;
    }
}
