using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton, quitButton;

    void AssignButtons()
    {
        playButton = GameObject.FindGameObjectWithTag("Play Button").GetComponent<Button>();
        optionsButton = GameObject.FindGameObjectWithTag("Options Button").GetComponent<Button>();
        quitButton = GameObject.FindGameObjectWithTag("Quit Button").GetComponent<Button>();
    }

    void AssignButtonCallbacks()
    {
        playButton.onClick.AddListener(GameManager.Instance.OnMenuEnd);

        optionsButton.onClick.AddListener(() => Debug.Log("Go to options!"));
        quitButton.onClick.AddListener(() => Debug.Log("Quit game!"));
    }

    private void Awake()
    {
        AssignButtons();
        AssignButtonCallbacks();
    }
}
