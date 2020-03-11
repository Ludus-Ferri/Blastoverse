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
    public float targetTimeScale;

    public float targetZoom;

    float defaultZoom;

    private Animator anim;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        anim = GetComponent<Animator>();

        defaultZoom = mainCamera.orthographicSize;

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
        StartCoroutine(UnscaledUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.backgroundColor = backgroundColor;
    }

    IEnumerator UnscaledUpdate()
    {
        while (true)
        {
            Time.timeScale = targetTimeScale;
            mainCamera.orthographicSize = targetZoom;

            yield return null;
        }
        
    }

    public void OnLoss()
    {
        anim.SetTrigger("DestructionPause");
    }
}
