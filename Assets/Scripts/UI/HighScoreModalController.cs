using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreModalController : MonoBehaviour
{
    public Animator anim;

    public TMP_Text deathText, scoreText, highScoreText;
    public int deathTextCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        SetDeathText();
        SetScoreTexts();

        anim.SetTrigger("Show");
    }

    public void Hide()
    {
        anim.SetTrigger("Hide");
    }

    void SetDeathText()
    {
        deathText.text = LocalizedStringManager.GetLocalizedString($"game.loss.{GenericRNG.Instance.Next(1, deathTextCount)}");
    }

    void SetScoreTexts()
    {
        scoreText.text = ScoreSystem.Instance.Score.ToString();

        if (GameManager.Instance.isHighScore)
            highScoreText.text = LocalizedStringManager.GetLocalizedString("ui.highScore");
        else highScoreText.text = "";
    }
}
