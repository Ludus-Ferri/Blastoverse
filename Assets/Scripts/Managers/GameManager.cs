using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameState;

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
    public float restartAsteroidRepelForce;
    public bool isHighScore;

    [Header("Particles")]
    public GameObject playerExplosionParticle;
    public GameObject playerReviveParticle;

    [Header("Music")]
    public LayeredBGM gameMusic;
    public LayeredBGM menuMusic;

    public float musicLossLowpass;
    public float musicLowpassSmoothing;

    public float musicLossVolume;
    public float musicVolumeSmoothing;

    [Range(0, 1)]
    public float musicIntensity;

    float defaultZoom;
    public float targetMusicCutoff, targetMusicVolume;

    [Header("Post Processing")]
    public VolumeProfile postProcessingProfile;

    private Animator anim;

    public delegate void OnGameReadyDelegate();
    public event OnGameReadyDelegate OnGameReady;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        anim = GetComponent<Animator>();

        defaultZoom = mainCamera.orthographicSize;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        mainCamera = Camera.main;

        LocalizedStringManager.Init();
        LocalizedStringManager.ParseTranslations();

        string currentCultureName = Localizer.GetCurrentCultureInfo().Name;
        Debug.LogFormat("Current culture: {0}", currentCultureName);

        LocalizedTextRegistry.Clear();

        Options.Load();
        Debug.LogFormat("Loading culture {0}", Options.currentCulture.Name);
        if (Options.currentCulture == null)
        {
            Debug.Log("Setting current culture!");
            if (!LocalizedStringManager.availableCultures.Contains(currentCultureName))
            {
                Debug.LogFormat("Culture {0} is not supported, defaulting to en-US");
                Options.currentCulture = CultureInfo.GetCultureInfo("en-US");
            }
        }
        LocalizedStringManager.SetCulture(Options.currentCulture.Name);
        Options.Save();

        targetTimeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UnscaledUpdate());
        StartCoroutine(AudioUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.backgroundColor = backgroundColor;
    }

    #region Update Routines

    void UpdateAudio()
    {
        LayeredBGMPlayer.Instance.intensity = musicIntensity;

        AudioManager.Instance.music.audioMixer.GetFloat("MusicLowpass", out float cutoff);
        AudioManager.Instance.music.audioMixer.SetFloat("MusicLowpass", MathHelper.Logerp(cutoff, targetMusicCutoff, Time.unscaledDeltaTime / musicLowpassSmoothing));

        AudioManager.Instance.music.audioMixer.GetFloat("MusicVolume", out float volume);
        volume += 100;
        AudioManager.Instance.music.audioMixer.SetFloat("MusicVolume", MathHelper.Logerp(volume, targetMusicVolume + 100, Time.unscaledDeltaTime / musicVolumeSmoothing) - 100);
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

    IEnumerator AudioUpdate()
    {
        while (true)
        {
            UpdateAudio();
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    #endregion

    #region Scene Load Events

    public void OnGameLoaded()
    {
        gameState = GameState.InGame; 
        FindGameObjects();

        UIManager.Instance.LocateHUDObjects();
        DifficultyManager.Instance.Init();
        ScoreSystem.Instance.Init();
        DifficultyManager.Instance.Setup();

        LayeredBGMPlayer.Instance.Stop();
        LayeredBGMPlayer.Instance.UnloadBGM();

        StartCoroutine(BeginGame());
        PlayGameMusic();
    }

    public void OnMenuLoaded()
    {
        gameState = GameState.MainMenu;

        LayeredBGMPlayer.Instance.Stop();
        LayeredBGMPlayer.Instance.UnloadBGM();

        StartCoroutine(BeginMenu());
        PlayMenuMusic();
    }

    public void OnSceneLoaded()
    {
        mainCamera = Camera.main;
    }

    public void OnQuitGame()
    {
        Debug.Log("Quitting!");
        Application.Quit();
    }

    #endregion

    #region Game Events
    public void OnLoss()
    {
        isHighScore = false;
        playerController.controlsEnabled = false;
        ScoreSystem.Instance.lockScore = true;

        targetMusicCutoff = musicLossLowpass;

        long highScore = HighScoreSystem.Instance.GetHighScore();

        if (highScore == -1)
        {
            HighScoreSystem.Instance.StoreScore(ScoreSystem.Instance.Score);
        }
        else
        {
            if (ScoreSystem.Instance.Score > highScore)
            {
                isHighScore = true;
                HighScoreSystem.Instance.StoreScore(ScoreSystem.Instance.Score);
            }
        }

        StartCoroutine(PlayerExplodeCutscene());

        anim.SetTrigger("DestructionPause");
            
    }

    public void OnRestart()
    {
        Debug.Log("Restarting!");
        StartCoroutine(RestartGame());
    }

    public void OnGameEnd()
    {
        StartCoroutine(EndGame());
    }

    #endregion

    #region UI Events
    public void OnMenuEnd()
    {
        StartCoroutine(EndMenu());
    }

    #endregion

    #region Cutscenes
    IEnumerator BeginGame()
    {
        targetMusicCutoff = 22000;
        targetMusicVolume = 0;

        ScoreSystem.Instance.lockScore = false;
        ScoreSystem.Instance.SetScore(0);
        ScoreSystem.Instance.lockScore = true;

        UIManager.Instance.highScoreModalController.gameObject.SetActive(false);
        UIManager.Instance.blackCutoutController.FadeToVisible();
        playerController.controlsEnabled = true;

        yield return new WaitForSecondsRealtime(0.9f);

        UIManager.Instance.topHUDController.Show();

        yield return new WaitForSecondsRealtime(0.2f);

        ScoreSystem.Instance.lockScore = false;
        OnGameReady?.Invoke();
    }

    IEnumerator BeginMenu()
    {
        targetMusicCutoff = 22000;
        targetMusicVolume = 0;

        yield return new WaitForSecondsRealtime(1f);

        UIManager.Instance.blackCutoutController.FadeToVisible();

        yield return null;
    }

    IEnumerator EndMenu()
    {
        targetMusicVolume = -80;

        UIManager.Instance.blackCutoutController.FadeToBlack();
        yield return new WaitForSecondsRealtime(0.7f);

        SceneManager.LoadScene("Game");
    }

    IEnumerator PlayerExplodeCutscene()
    {
        AudioManager.Instance.PlaySoundAtPosition(AudioManager.Instance.GetSound("Player Explosion"), playerController.transform.position);

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
        StartCoroutine(GameOverCutscene());

    }

    IEnumerator GameOverCutscene()
    {
        UIManager.Instance.topHUDController.Hide();
        yield return new WaitForSecondsRealtime(1.7f);
        targetMusicVolume = musicLossVolume;
        UIManager.Instance.highScoreModalController.gameObject.SetActive(true);
        UIManager.Instance.highScoreModalController.Show();
    }

    IEnumerator RestartGame()
    { 
        CameraShake shake = mainEffectManager.GetEffect<CameraShake>();

        UIManager.Instance.highScoreModalController.Hide();
        yield return new WaitForSecondsRealtime(0.5f);
        UIManager.Instance.highScoreModalController.gameObject.SetActive(false);

        foreach (GameObject asteroid in asteroidPool.objects)
        {
            if (asteroid.activeInHierarchy)
            {
                Vector2 force = new Vector2(asteroid.transform.position.x, asteroid.transform.position.y).normalized;

                Rigidbody2D rb2d = asteroid.GetComponent<Rigidbody2D>();
                rb2d.velocity = force * restartAsteroidRepelForce;
                rb2d.AddTorque(GenericRNG.Instance.NextFloat(-1, 1) * restartAsteroidRepelForce * 30);
            }
        }

        Instantiate(playerReviveParticle, playerController.transform.position, Quaternion.identity);

        playerController.gameObject.SetActive(true);
        playerController.transform.rotation = Quaternion.Euler(0, 0, 90);
        playerController.gameObject.SetActive(false);

        float time = Time.unscaledTime;

        while (Time.unscaledTime - time < 1.6f)
        {
            shake.InduceMotion(0.7f * Time.unscaledDeltaTime);
            yield return null;
        }

        targetMusicCutoff = 22000;
        targetMusicVolume = 0;

        playerController.gameObject.SetActive(true);
        playerController.controlsEnabled = true;
        playerController.isDead = false;

        shake.InduceMotion(2f);

        yield return new WaitForSecondsRealtime(0.6f);

        UIManager.Instance.topHUDController.Show();

        ScoreSystem.Instance.lockScore = false;
        ScoreSystem.Instance.SetScore(0);

        OnGameReady?.Invoke();
    }



    IEnumerator EndGame()
    {
        targetMusicVolume = -80;
        yield return new WaitForSecondsRealtime(0.7f);

        UIManager.Instance.blackCutoutController.FadeToBlack();
        yield return new WaitForSecondsRealtime(0.7f);

        SceneManager.LoadScene("NewMenu");
    }
    #endregion

    #region Music
    void PlayGameMusic()
    {
        LayeredBGMPlayer.Instance.currentBGM = gameMusic;
        LayeredBGMPlayer.Instance.LoadBGM();
        LayeredBGMPlayer.Instance.Play();
    }

    void PlayMenuMusic()
    {
        LayeredBGMPlayer.Instance.currentBGM = menuMusic;
        LayeredBGMPlayer.Instance.LoadBGM();
        LayeredBGMPlayer.Instance.Play();
    }
    #endregion

    #region Miscellaneous

    void FindGameObjects()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerEnergySystem = player.GetComponent<PlayerEnergySystem>();

        mainCamera = Camera.main;
        mainEffectManager = mainCamera.GetComponent<CameraEffectManager>();

        asteroidPool = GameObject.FindGameObjectWithTag("Asteroid Pool").GetComponent<ObjectPooler>();
    }

    #endregion
}
