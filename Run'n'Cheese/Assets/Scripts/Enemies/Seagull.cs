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

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        bool _isPushZone = collider.gameObject.GetComponent<PushZone>() != null;
        if (!_isPushZone) return;
        
        OnKill?.Invoke();        
    }
}
