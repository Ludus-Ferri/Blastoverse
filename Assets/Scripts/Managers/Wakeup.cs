using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wakeup : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.OnGameLoaded();
    }
}
