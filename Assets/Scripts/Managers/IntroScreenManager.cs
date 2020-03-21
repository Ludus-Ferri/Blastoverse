using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenManager : MonoBehaviour
{
    public Animator anim;
    public AudioSource jingle;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        anim.Play("Flash");

        yield return new WaitForSecondsRealtime(0.1f);
        jingle.Play();

        yield return new WaitForSecondsRealtime(2.2f);

        SceneManager.LoadScene("NewMenu");
    }
}
