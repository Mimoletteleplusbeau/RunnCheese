using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TemporaryPlateform : DestructiblePlateform
{
    public event Action OnDeath;
    private Collider2D _collider;
    [SerializeField] private float _destroyTime;
    [SerializeField] private ShakeTransform _shakeVisual;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        transform.localScale = Vector2.zero;
    }

    private void Start()
    {
        transform.DOScale(1, 0.5f);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            if (collision.gameObject.transform.position.y < transform.position.y + _collider.bounds.extents.y) return;
            if (collision.gameObject.transform.position.x > transform.position.x + _collider.bounds.extents.x) return;
            if (collision.gameObject.transform.position.x < transform.position.x - _collider.bounds.extents.x) return;
            _shakeVisual.Begin();
            StartCoroutine(Break(_destroyTime));
            return;
        }

        if (collision.gameObject.GetComponent<Bullet>() == null) return;
        base.OnCollisionEnter2D(collision);
        StopAllCoroutines();
        OnDeath?.Invoke();
    }

    IEnumerator Break(float time)
    {
        yield return new WaitForSeconds(time);
        var myExplosion = GetComponent<ExplosionObject>();
        if (myExplosion != null) myExplosion.Explode();
        else Destroy(gameObject);
        OnDeath?.Invoke();
    }
}
