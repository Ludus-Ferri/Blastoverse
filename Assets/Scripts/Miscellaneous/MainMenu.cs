using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void Back()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene("TestScene");
    }
}
