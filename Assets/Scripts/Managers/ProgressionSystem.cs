using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem : MonoBehaviour
{
    public static ProgressionSystem Instance;

    public List<ProgressionStep> progressionSteps;

    ProgressionStep currentStep;

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

    // Update is called once per frame
    void Update()
    {
        foreach (ProgressionStep step in progressionSteps)
        {
            if (ScoreSystem.Instance.Score > step.scoreThreshold)
            {
                currentStep = step;
                break;
            }
        }
    }
}
