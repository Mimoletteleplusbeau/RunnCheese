using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlateform : DestructiblePlateform
{
    [SerializeField] private float _destroyTime;
    public event Action OnDeath;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Destroy(gameObject, _destroyTime);
            return;
        }
        base.OnCollisionEnter2D(collision);
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
    }
}
