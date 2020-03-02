using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moov : MonoBehaviour
{
    double angle;
    public int Bulletspeed = 10;
    Rigidbody2D RB2D;
    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.AddForce(new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z), Mathf.Sin(transform.rotation.eulerAngles.z)) *Bulletspeed);
        angle = transform.rotation.z; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,90*Time.deltaTime);
    }
}
