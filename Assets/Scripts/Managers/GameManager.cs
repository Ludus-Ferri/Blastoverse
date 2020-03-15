using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int screenWidth, screenHeight;

    public Camera mainCamera;
    public CameraEffectManager mainEffectManager;

    public PlayerController playerController;
    public PlayerEnergySystem playerEnergySystem;

    public ObjectPooler asteroidPool;

    [Header("Level Properties")]
    public Color backgroundColor;

    [Header("Game Mechanics")]
    public float timeScaleSmoothing;
    public float targetTimeScale;
    public float targetZoom;

    public float playerExplosionAsteroidRepelForce;

    [Header("Particles")]
    public GameObject playerExplosionParticle;

    float defaultZoom;

    private Animator anim;

    public delegate void OnGameReadyDelegate();
    public event OnGameReadyDelegate OnGameReady;

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
        BeginGame();
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

    public void OnGameLoaded()
    {
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        ScoreSystem.Instance.lockScore = true;
        ScoreSystem.Instance.SetScore(0);

        UIManager.Instance.LocateHUDObjects();

#if UNITY_EDITOR
        yield return new WaitForSecondsRealtime(1f);
#endif

        UIManager.Instance.blackCutoutController.FadeToVisible();
        playerController.controlsEnabled = true;

        yield return new WaitForSecondsRealtime(0.9f);

        UIManager.Instance.topHUDController.Show();

        yield return new WaitForSecondsRealtime(0.2f);

        ScoreSystem.Instance.lockScore = false;
        OnGameReady?.Invoke();
    }

    public void OnLoss()
    {
        playerController.controlsEnabled = false;
        ScoreSystem.Instance.lockScore = true;

        StartCoroutine(PlayerExplodeCutscene());

        anim.SetTrigger("DestructionPause");
    }

    IEnumerator PlayerExplodeCutscene()
    {
        float time = Time.unscaledTime;

        CameraShake shake = mainEffectManager.GetEffect<CameraShake>();

        while (Time.unscaledTime - time < 1.9f)
        {
            shake.InduceMotion(0.5f * Time.unscaledDeltaTime);
            yield return null;
        }

        Instantiate(playerExplosionParticle, playerController.transform.position + Vector3.forward * -1, Quaternion.identity);
        playerController.gameObject.SetActive(false);

        foreach (GameObject obj in asteroidPool.objects.Where(o => o.activeInHierarchy))
        {
            Vector3 distance = obj.transform.position - playerController.transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(distance.normalized * playerExplosionAsteroidRepelForce / distance.magnitude);
        }

        shake.InduceMotion(3f);
        yield return null;
    }
}
