using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager Instance;
    [SerializeField] private LevelsList _levelsList;
    private int _currentLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += GetCurrentLevel;
    }

    private void GetCurrentLevel(Scene scene, LoadSceneMode mode)
    {
        scene = SceneManager.GetActiveScene();

        for (int i = 0; i < _levelsList.Levels.Length; i++)
        {
            if (scene.name == _levelsList.Levels[i].Name)
            {
                _currentLevel = i;
                break;
            }
        }

    }

    public void GoToNextLevel()
    {
        if (_currentLevel + 1 >= _levelsList.Levels.Length)
        {
            GoToMenu();
            return;
        }
        Transition.Instance.SetTransition(DirectlyGoToNextLevel);
    }
    public void DirectlyGoToNextLevel()
    {
        if (_currentLevel + 1 >= _levelsList.Levels.Length)
        {
            DirectlyGoToMenu();
            return;
        }
        SceneManager.LoadScene(_levelsList.Levels[_currentLevel + 1].Name);
    }

    public void RestartLevel()
    {
        Transition.Instance.SetTransition(DirectlyRestartLevel);
    }

    public void DirectlyRestartLevel()
    {
        SceneManager.LoadScene(_levelsList.Levels[_currentLevel].Name);
    }

    public void RestartAfterTime(float time)
    {
        StartCoroutine(WaitForRestart(time));
    }

    private IEnumerator WaitForRestart(float time)
    {
        yield return new WaitForSeconds(time);
        RestartLevel();
    }

    public void GoToMenu()
    {
        _currentLevel = 0;
        Transition.Instance.SetTransition(DirectlyGoToMenu);
    }

    public void DirectlyGoToMenu()
    {
        SceneManager.LoadScene(_levelsList.MainMenu);
    }

    public float[] GetFishTimer()
    {
        float fishTimer1 = _levelsList.Levels[_currentLevel].FishTimer1;
        float fishTimer2 = _levelsList.Levels[_currentLevel].FishTimer2;
        float fishTimer3 = _levelsList.Levels[_currentLevel].FishTimer3;
        float[] fishTimers = new float[] { fishTimer1, fishTimer2, fishTimer3 };
        return fishTimers;
    }
}
