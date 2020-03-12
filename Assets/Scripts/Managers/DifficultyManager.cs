using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public AsteroidGenerator asteroidGen;

    public static DifficultyManager Instance;
    public enum Level
    {
        Very_Easy, Easy, Normal, Hard, Pro
    }
    Dictionary<Level, int> maxEnergies;
    Dictionary<Level, int> energyRegens;
    Dictionary<Level, int> regenAccels;
    Dictionary<Level, float> spawnTime;
    Dictionary<Level, int> minVert;
    Dictionary<Level, int> maxVert;
    public Level currentLevel;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        maxEnergies = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 1900 },
            { Level.Easy, 1500 },
            { Level.Normal, 1200 },
            { Level.Hard, 900 },
            { Level.Pro, 600 }
        };

        energyRegens = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 50 },
            { Level.Easy, 40 },
            { Level.Normal, 30 },
            { Level.Hard, 25 },
            { Level.Pro, 20 }
        };

        regenAccels = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 15 },
            { Level.Easy, 12 },
            { Level.Normal, 10 },
            { Level.Hard, 9 },
            { Level.Pro, 7 }
        };

        spawnTime = new Dictionary<Level, float>()
        {
            { Level.Very_Easy, 3f },
            { Level.Easy, 2.1f },
            { Level.Normal, 1.8f },
            { Level.Hard, 1.5f },
            { Level.Pro, 1f }
        };

        minVert = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 3 },
            { Level.Easy, 4 },
            { Level.Normal, 5 },
            { Level.Hard, 5 },
            { Level.Pro, 6 }
        };

        maxVert = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 5 },
            { Level.Easy, 7 },
            { Level.Normal, 8 },
            { Level.Hard, 9 },
            { Level.Pro, 11 }
        };


    }
    
    void Start()
    {
        GameManager.Instance.playerEnergySystem.maxEnergy = maxEnergies[(Level)currentLevel];
        GameManager.Instance.playerEnergySystem.energyRegeneration = energyRegens[(Level)currentLevel];
        GameManager.Instance.playerEnergySystem.energyRegenerationAccel = regenAccels[(Level)currentLevel];
        asteroidGen.spawnRate = spawnTime[(Level)currentLevel];
        asteroidGen.minimumVertices = minVert[(Level)currentLevel];
        asteroidGen.maximumVertices = maxVert[(Level)currentLevel];
        ScoreSystem.Instance.difficulty = ((int)currentLevel+1)*((int)currentLevel+1) - (int)currentLevel*2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}