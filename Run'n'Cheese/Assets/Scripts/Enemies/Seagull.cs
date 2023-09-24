using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : Enemy
{
    public static Seagull Instance;
    public event Action OnKill;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        OnKill?.Invoke();
    }
}
