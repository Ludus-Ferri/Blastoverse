using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    
    public static DifficultyManager Instance;
    public enum Level
    {
        Very_Easy, Easy, Normal, Hard, Pro
    }
    Dictionary<Level, int> MaxEnergies;
    Dictionary<Level, int> EnergyRegens;

    Dictionary<Level, int> RegenAccels;
    public Level currentLevel;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        MaxEnergies = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 1900 },
            { Level.Easy, 1500 },
            { Level.Normal, 1200 },
            { Level.Hard, 900 },
            { Level.Pro, 600 }
        };

        EnergyRegens = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 50 },
            { Level.Easy, 40 },
            { Level.Normal, 30 },
            { Level.Hard, 25 },
            { Level.Pro, 20 }
        };

        RegenAccels = new Dictionary<Level, int>()
        {
            { Level.Very_Easy, 15 },
            { Level.Easy, 12 },
            { Level.Normal, 10 },
            { Level.Hard, 9 },
            { Level.Pro, 7 }
        };
    }
    
    void Start()
    {
        GameManager.Instance.playerEnergySystem.maxEnergy = MaxEnergies[(Level)currentLevel];
        GameManager.Instance.playerEnergySystem.energyRegeneration = EnergyRegens[(Level)currentLevel];
        GameManager.Instance.playerEnergySystem.energyRegenerationAccel = RegenAccels[(Level)currentLevel];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
