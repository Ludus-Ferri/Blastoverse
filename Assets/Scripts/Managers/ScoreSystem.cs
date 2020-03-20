using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    double score;
    public long Score
    {
        get
        {
            return (long)score;
        }
    }

    [Header("Score Growth Properties")]
    public int scorePerSecond;
    public float scoreMultiplier;
    
    public int difficulty;
    public bool lockScore;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public Color zeroColor;
    public int zeroPadding = 8;

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

    public void Init()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score Text").GetComponent<TMP_Text>();
    }

    public void SetScore(long val)
    {
        if (!lockScore)
            score = val;
    }

    public void IncreaseScore(long val)
    {
        if (!lockScore)
            score += val * scoreMultiplier * difficulty;
    }

    public void DecreaseScore(long val)
    {
        if (!lockScore)
        {
            score -= val * scoreMultiplier;
            if (score < 0) score = 0;
        }   
    }

    void UpdateScoreText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"<color=#{ColorUtility.ToHtmlStringRGBA(zeroColor)}>");

        if (Score.ToString().Length >= zeroPadding)
        {
            builder.Append("</color>");
            builder.Append(Score.ToString($"D{zeroPadding}"));
        }
        else
        {
            builder.Append(Score.ToString($"D{zeroPadding}"));
            for (int i = 18; i < builder.Length; i++)
            {
                if (builder[i] != '0' && builder[i - 1] == '0')
                {
                    builder.Insert(i, "</color>");
                    break;
                }
            }
        }

        scoreText.text = builder.ToString();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.InGame)
        {
            if (!lockScore)
                score += scorePerSecond * scoreMultiplier * Time.deltaTime * difficulty;

            UpdateScoreText();
        }
        
    }
}
