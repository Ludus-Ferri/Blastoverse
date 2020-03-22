using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvasController : MonoBehaviour
{
    public static PersistentCanvasController Instance;

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
}
