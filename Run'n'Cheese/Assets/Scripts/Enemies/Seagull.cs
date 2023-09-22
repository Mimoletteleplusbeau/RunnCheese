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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool _isGlobFish = collision.gameObject.GetComponent<Blobfish>() != null || collision.gameObject.GetComponent<PushZone>() != null;
        if (_isGlobFish)
        {
            OnKill?.Invoke();
        }
    }
}
