using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;
    public AsteroidGenerator asteroidGen;

    public enum Difficulty
    {
        VeryEasy, Easy, Normal, Hard, Pro
    }

    Dictionary<Difficulty, int> maxEnergies;
    Dictionary<Difficulty, int> energyRegens;
    Dictionary<Difficulty, int> regenAccels;
    Dictionary<Difficulty, float> spawnTime;
    Dictionary<Difficulty, int> minVert;
    Dictionary<Difficulty, int> maxVert;

    public Difficulty currentDifficulty;
    
    // Start is called before the first frame update
    void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        maxEnergies = new Dictionary<Difficulty, int>()
        {
            { Difficulty.VeryEasy, 1900 },
            { Difficulty.Easy, 1500 },
            { Difficulty.Normal, 1200 },
            { Difficulty.Hard, 900 },
            { Difficulty.Pro, 600 }
        };

        energyRegens = new Dictionary<Difficulty, int>()
        {
            { Difficulty.VeryEasy, 50 },
            { Difficulty.Easy, 40 },
            { Difficulty.Normal, 30 },
            { Difficulty.Hard, 25 },
            { Difficulty.Pro, 20 }
        };

        regenAccels = new Dictionary<Difficulty, int>()
        {
            { Difficulty.VeryEasy, 15 },
            { Difficulty.Easy, 12 },
            { Difficulty.Normal, 10 },
            { Difficulty.Hard, 9 },
            { Difficulty.Pro, 7 }
        };

        spawnTime = new Dictionary<Difficulty, float>()
        {
            { Difficulty.VeryEasy, 3f },
            { Difficulty.Easy, 2.1f },
            { Difficulty.Normal, 1.8f },
            { Difficulty.Hard, 1.5f },
            { Difficulty.Pro, 1f }
        };

        minVert = new Dictionary<Difficulty, int>()
        {
            { Difficulty.VeryEasy, 3 },
            { Difficulty.Easy, 4 },
            { Difficulty.Normal, 5 },
            { Difficulty.Hard, 5 },
            { Difficulty.Pro, 6 }
        };

        maxVert = new Dictionary<Difficulty, int>()
        {
            { Difficulty.VeryEasy, 5 },
            { Difficulty.Easy, 7 },
            { Difficulty.Normal, 8 },
            { Difficulty.Hard, 9 },
            { Difficulty.Pro, 11 }
        };
    }
    
    void Start()
    {
        if (GameManager.Instance.gameState == GameState.InGame)
            Setup();
    }

    public void Init()
    {
        asteroidGen = GameObject.FindGameObjectWithTag("Asteroid Generator").GetComponent<AsteroidGenerator>();
    }

    public void Setup()
    {
        GameManager.Instance.playerEnergySystem.maxEnergy = maxEnergies[currentDifficulty];
        GameManager.Instance.playerEnergySystem.energyRegeneration = energyRegens[currentDifficulty];
        GameManager.Instance.playerEnergySystem.energyRegenerationAccel = regenAccels[currentDifficulty];

        asteroidGen.spawnRate = spawnTime[currentDifficulty];
        asteroidGen.minimumVertices = minVert[currentDifficulty];
        asteroidGen.maximumVertices = maxVert[currentDifficulty];

        ScoreSystem.Instance.difficulty = ((int)currentDifficulty + 1) * ((int)currentDifficulty + 1) - (int)currentDifficulty * 2;
    }
}