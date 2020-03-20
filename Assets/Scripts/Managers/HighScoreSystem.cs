using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This uses PlayerPrefs instead of BinaryFormatter
// Reason 1: PlayerPrefs is simpler
// Reason 2: BinaryFormatter sometimes has issues on Android
// Reason 3: yes
public class HighScoreSystem : MonoBehaviour
{
    public static HighScoreSystem Instance;

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

    public long GetHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            return long.Parse(PlayerPrefs.GetString("HighScore"));
        return -1;
    }

    public void StoreScore(long score)
    {
        PlayerPrefs.SetString("HighScore", score.ToString());
        PlayerPrefs.Save();
    }
}
