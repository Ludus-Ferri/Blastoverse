using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private Animator anim;

    public Button resumeBtn, quitBtn;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetVisible(bool visible)
    {
        anim.SetTrigger(visible ? "Show" : "Hide");
    }

    public void OnResumeButtonPressed()
    {
        anim.SetTrigger("Hide");
        GameManager.Instance.OnPauseUnpause();
    }

    public void OnQuitButtonPressed()
    {
        anim.SetTrigger("Hide");
        GameManager.Instance.OnGameEnd();
    }
}
