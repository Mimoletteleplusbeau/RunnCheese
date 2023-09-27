using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlateform : DestructiblePlateform
{
    private Collider2D _collider;
    [SerializeField] private float _destroyTime;
    [SerializeField] private ShakeTransform _shakeVisual;
    public event Action OnDeath;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (collision.gameObject.transform.position.y < transform.position.y + _collider.bounds.extents.y) return;
            if (collision.gameObject.transform.position.x > transform.position.x + _collider.bounds.extents.x) return;
            if (collision.gameObject.transform.position.x < transform.position.x - _collider.bounds.extents.x) return;
            _shakeVisual.Begin();
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
