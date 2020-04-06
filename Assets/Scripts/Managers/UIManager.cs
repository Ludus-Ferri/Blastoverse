using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public EnergyBarController energyBarController;
    public TopHUDController topHUDController;
    public BlackCutoutController blackCutoutController;
    public HighScoreModalController highScoreModalController;
    public PauseMenuManager pauseMenuManager;

    public Button restartBtn, giveUpBtn, pauseBtn;

    public void LocateHUDObjects()
    {
        energyBarController = GameObject.FindGameObjectWithTag("Energy Bar").GetComponent<EnergyBarController>();
        topHUDController = GameObject.FindGameObjectWithTag("Top HUD").GetComponent<TopHUDController>();
        highScoreModalController = GameObject.FindGameObjectWithTag("High Score Modal").GetComponent<HighScoreModalController>();
        pauseMenuManager = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenuManager>();

        restartBtn = GameObject.FindGameObjectWithTag("Restart Button").GetComponent<Button>();
        giveUpBtn = GameObject.FindGameObjectWithTag("Give Up Button").GetComponent<Button>();
        pauseBtn = GameObject.FindGameObjectWithTag("Pause Button").GetComponent<Button>();

        restartBtn.onClick.AddListener(GameManager.Instance.OnRestart);
        giveUpBtn.onClick.AddListener(GameManager.Instance.OnGameEnd);
        pauseBtn.onClick.AddListener(OnPauseButtonClicked);

    }

    private void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.InGame)
            energyBarController.value = GameManager.Instance.playerEnergySystem.energy / GameManager.Instance.playerEnergySystem.maxEnergy;
    }

    public void OnPauseButtonClicked()
    {
        GameManager.Instance.OnPauseUnpause();
    }
}
