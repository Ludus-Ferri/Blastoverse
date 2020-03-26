using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button optionsButton, quitButton;

    public Animator mainAnim, optionsAnim;

    void AssignButtons()
    {
        playButton = GameObject.FindGameObjectWithTag("Play Button").GetComponent<Button>();
        optionsButton = GameObject.FindGameObjectWithTag("Options Button").GetComponent<Button>();
        quitButton = GameObject.FindGameObjectWithTag("Quit Button").GetComponent<Button>();
    }

    void AssignButtonCallbacks()
    {
        playButton.onClick.AddListener(GameManager.Instance.OnMenuEnd);

        optionsButton.onClick.AddListener(ShowOptionsMenu);
        quitButton.onClick.AddListener(GameManager.Instance.OnQuitGame);
    }

    private void Awake()
    {
        AssignButtons();
        AssignButtonCallbacks();
    }

    public void ShowOptionsMenu()
    {
        StartCoroutine(AnimateShowOptions());
    }

    IEnumerator AnimateShowOptions()
    {
        mainAnim.SetTrigger("Hide");
        yield return new WaitForSecondsRealtime(0.1f);
        optionsAnim.SetTrigger("Show");
    }

    public void HideOptionsMenu()
    {
        StartCoroutine(AnimateHideOptions());
    }

    IEnumerator AnimateHideOptions()
    {
        optionsAnim.SetTrigger("Hide");
        yield return new WaitForSecondsRealtime(0.1f);
        mainAnim.SetTrigger("Show");
    }
}
