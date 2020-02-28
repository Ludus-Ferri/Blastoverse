using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    public float score;

    [Header("Score Growth Propeties")]
    public int scorePerSecond;
    public float scoreMultiplier;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
    }

    public void IncreaseScore(int val)
    {
        score += Mathf.RoundToInt(val * scoreMultiplier);
    }

    public void DecreaseScore(int val)
    {
        score -= Mathf.RoundToInt(val * scoreMultiplier);
        if (score < 0) score = 0;
    }

    private void Update()
    {
        score += scorePerSecond * Time.deltaTime;
    }
}
