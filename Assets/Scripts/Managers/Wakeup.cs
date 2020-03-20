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
                GameManager.Instance.OnGameLoaded();
                break;
            case GameState.MainMenu:
                GameManager.Instance.OnMenuLoaded();
                break;
            default: break;

        }
    }
}
