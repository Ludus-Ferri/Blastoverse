using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public float energy;
    public float shot_cost;
    public float ability_cost;
    public float energy_per_sec;
    public float max_energy;
    public float cooldown;
    float last_shot_time;
    //public float game_time;

    // Start is called before the first sec update
    void Start()
    {
        
    }

    // Update is called once per sec
    void Update()
    {
        if (Time.time - last_shot_time > cooldown)
        {
            if (energy+energy_per_sec*Time.deltaTime <= max_energy)
            {
                energy += energy_per_sec * Time.deltaTime;
            }
            else
            {
                energy = max_energy;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            energy -= shot_cost;
            last_shot_time = Time.time;
        }
        

    }
}
