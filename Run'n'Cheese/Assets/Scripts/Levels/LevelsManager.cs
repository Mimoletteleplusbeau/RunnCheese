using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager Instance;
    [SerializeField] private LevelsList _levelsList;
    public int _currentLevel;

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

    public void GoToNextLevel()
    {
        Debug.Log("levelsManagerNextLevel");
        Transition.Instance.SetTransition(DirectlyGoToNextLevel);
    }

    public void RestartLevel()
    {
        Transition.Instance.SetTransition(DirectlyRestartLevel);
    }

    public void DirectlyRestartLevel()
    {
        SceneManager.LoadScene(_levelsList.Levels[_currentLevel]);
    }

    public void DirectlyGoToNextLevel()
    {
        Debug.Log(_currentLevel);
        _currentLevel++;
        Debug.Log(_currentLevel);
        SceneManager.LoadScene(_levelsList.Levels[_currentLevel+1]);
    }

    public void GoToMenu()
    {
        _currentLevel = 0;
        Transition.Instance.SetTransition(() => SceneManager.LoadScene(_levelsList.MainMenu));
    }
}
