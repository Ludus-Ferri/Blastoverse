using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wakeup : MonoBehaviour
{
    public GameState gameState;

    private void Awake()
    {
        switch (gameState)
        {
            case GameState.InGame:
                GameManager.Instance.OnSceneLoaded();
                GameManager.Instance.OnGameLoaded();
                break;
            case GameState.MainMenu:
                GameManager.Instance.OnSceneLoaded();
                GameManager.Instance.OnMenuLoaded();
                break;
            default: break;

        }
    }
}
