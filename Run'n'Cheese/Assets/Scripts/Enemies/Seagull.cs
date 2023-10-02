using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : Enemy
{
    public static Seagull Instance;

    public event Action OnKill;
    [field: SerializeField] protected override GameObject _VFXDeath { get; set; }
    [field: SerializeField] protected override AudioClip _SFXDeath { get; set; }


    private void Awake()
    {
        Instance = this;
    }

    protected override void OnDestroy()
    {
        OnKill?.Invoke();
        base.OnDestroy();
    }
}
