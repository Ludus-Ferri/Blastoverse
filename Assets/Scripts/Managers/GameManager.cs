using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int screenWidth, screenHeight;

    public Camera mainCamera;
    public CameraEffectManager mainEffectManager;

    public PlayerController playerController;
    public PlayerEnergySystem playerEnergySystem;

    [Header("Level Properties")]
    public Color backgroundColor;

    [Header("Game Mechanics")]
    public float timeScaleSmoothing;
    float targetTimeScale;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        //mainCamera = Camera.main;

        LocalizedStringManager.Init();
        LocalizedStringManager.ParseTranslations();
        LocalizedStringManager.SetCulture("pl-PL");

        targetTimeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.backgroundColor = backgroundColor;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, timeScaleSmoothing * Time.unscaledDeltaTime);
    }

    public void OnLoss()
    {
        targetTimeScale = 0;
    }
}
