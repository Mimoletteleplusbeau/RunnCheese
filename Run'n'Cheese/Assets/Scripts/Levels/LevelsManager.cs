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

    public void GoToNextLevel()
    {
        Transition.Instance.SetTransition(() => SceneManager.LoadScene(_currentLevel + 1));
    }

    public void RestartLevel()
    {
        //Transition.Instance.SetTransition(() => SceneManager.LoadScene(_levelsList._currentLevel));
    }
}
