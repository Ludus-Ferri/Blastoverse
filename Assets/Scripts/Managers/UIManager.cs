using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public EnergyBarController energyBarController;
    public BlackCutoutController blackCutoutController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        energyBarController.value = GameManager.Instance.playerEnergySystem.energy / GameManager.Instance.playerEnergySystem.maxEnergy;
    }
}
