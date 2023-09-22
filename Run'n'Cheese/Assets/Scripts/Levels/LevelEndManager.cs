using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance;
    public event Action OnLevelWin;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (Seagull.Instance != null) 
            Seagull.Instance.OnKill += LevelEnd;
    }

    private void OnDisable()
    {
        Seagull.Instance.OnKill -= LevelEnd;
    }

    private void LevelEnd()
    {
        OnLevelWin?.Invoke();
    }
}
