using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergySystem : MonoBehaviour
{
    public float energy;

    public float shotCost;
    public float abilityCost;
    public float energyRegeneration;
    public float energyRegenerationAccel;
    public float maxEnergy;
    public float cooldown;

    float lastShotTime;
    float regeneration;

    private void Start()
    {
        GameManager.Instance.playerController.OnShoot += PlayerController_OnShoot;
        regeneration = energyRegeneration;
    }

    private void PlayerController_OnShoot()
    {
        energy -= shotCost;
        lastShotTime = Time.time;
        regeneration = energyRegeneration;

        UIManager.Instance.energyBarController.Flash();
    }

    // Update is called once per sec
    void Update()
    {
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        if (Time.time - lastShotTime > cooldown)
        {
            if (energy + regeneration * Time.deltaTime <= maxEnergy)
            {
                energy += regeneration * Time.deltaTime;
                regeneration += energyRegenerationAccel * Time.deltaTime;
            }
            else
            {
                energy = maxEnergy;
            }
        }
    }
}
