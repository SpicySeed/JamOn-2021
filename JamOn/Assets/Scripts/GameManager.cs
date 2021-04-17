using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private CollectableManager collectableManager;
    [SerializeField] private DeathManager deathManager;
    [SerializeField] private TimeCountManager timeCountManager;

    private bool timerEnabled = true;

    private int numDeaths = 0;
    private int currentLevel = 1;
    private bool loading = false;
    private float time = 0.0f;

    private bool inputFreeze = false;
    private bool timerStopped = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            time = 0.0f;
            if (Instance.timeCountManager != null) Instance.timerStopped = false;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Instance.transitionManager = this.transitionManager;
        Instance.collectableManager = this.collectableManager;
        Instance.deathManager = this.deathManager;
        Instance.deathManager.PlayerDeath(numDeaths);
        Instance.timeCountManager = this.timeCountManager;
        if (Instance.timeCountManager != null) Instance.timerStopped = false;
        Instance.loading = false;
        Destroy(gameObject);
    }

    public void LoadScene(int index)
    {
        if (loading) return;

        loading = true;
        StartCoroutine(transitionManager.StartTransitionAndLoad(TransitionManager.Transitions.FADE, index));
    }

    public void LoadNextLevel()
    {
        if (loading) return;

        loading = true;
        currentLevel++;
        StartCoroutine(transitionManager.StartTransitionAndLoad(TransitionManager.Transitions.FADE, currentLevel));
    }

    public void ResetCurrentLevel()
    {
        currentLevel = 1;
    }

    public void ObjectCollected()
    {
        collectableManager.ObjectCollected();
    }

    public void CollectableReset()
    {
        collectableManager.ObjectReset();
    }

    public void PlayerDeath()
    {
        numDeaths++;
        deathManager.PlayerDeath(numDeaths);
    }

    public void SetTimedModeActive(bool active)
    {
        timerEnabled = active;
    }

    public bool IsTimeModeActive()
    {
        return timerEnabled;
    }

    public TransitionManager GetTransitionManager()
    {
        return transitionManager;
    }

    public void SetInputFreeze(bool freeze)
    {
        inputFreeze = freeze;
    }

    public bool GetInputFreeze()
    {
        return inputFreeze;
    }

    public void StopTimer(bool stopped)
    {
        timerStopped = stopped;
    }

    private void Update()
    {
        if (timeCountManager != null && timerEnabled && !timerStopped)
        {
            time += Time.deltaTime;
            timeCountManager.SetTime(time);
        }
    }
}
