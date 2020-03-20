using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public EnergyBarController energyBarController;
    public TopHUDController topHUDController;
    public BlackCutoutController blackCutoutController;
    public HighScoreModalController highScoreModalController;

    public void LocateHUDObjects()
    {
        energyBarController = GameObject.FindGameObjectWithTag("Energy Bar").GetComponent<EnergyBarController>();
        topHUDController = GameObject.FindGameObjectWithTag("Top HUD").GetComponent<TopHUDController>();
        highScoreModalController = GameObject.FindGameObjectWithTag("High Score Modal").GetComponent<HighScoreModalController>();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.InGame)
            energyBarController.value = GameManager.Instance.playerEnergySystem.energy / GameManager.Instance.playerEnergySystem.maxEnergy;
    }
}
