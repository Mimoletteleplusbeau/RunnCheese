using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gull : Enemy
{
    [Tooltip("The speed the gull moves at")] [SerializeField] private float _speed;
    [Tooltip("The direction the gull goes in (-1 or 1)")] [SerializeField] private float _direction = 1;
    [Tooltip("The radius of the cirlce the gull follows")] [SerializeField] private float _radius;
    private float _angle;
    private Vector3 _pivotPoint;
    [Tooltip("The time after which the gull starts moving (for more randomness)")] [SerializeField] private float _delay;
    private float _currentDelay;

    [Header("Random")]
    [Tooltip("Give random stats")] [SerializeField] private bool _isRandom;
    [Range(0, 1)] [Tooltip("The min stats multiplier")] [SerializeField] float _minRandomRange = 1;
    [Range(1, 5)] [Tooltip("The max stats multiplier")] [SerializeField] float _maxRandomRange = 1;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [field: SerializeField] protected override GameObject _VFXDeath { get; set; }
    [field: SerializeField] protected override AudioClip _SFXDeath { get; set; }



    private void Start()
    {
        _pivotPoint = transform.position;
        _currentDelay = _delay;
        PutRandomValues();
    }
    private void Update()
    {
        _currentDelay -= Time.deltaTime;
        if (_currentDelay > 0) return;
        _angle += _speed * Time.deltaTime;
        Vector3 direction = Quaternion.Euler(Vector3.forward * _angle * _direction) * _pivotPoint;
        transform.position = direction.normalized * _radius;
        transform.position += _pivotPoint;
        Debug.DrawLine(_pivotPoint, transform.position);

        _spriteRenderer.flipX = _angle % 360 > 180;
    }

    private void PutRandomValues()
    {
        if (_isRandom)
        {
            _speed = Random.Range(_speed * _minRandomRange, _speed * _maxRandomRange);
            _direction = Mathf.Floor(Random.Range(0, 2)) * 2 - 1;
            _radius = Random.Range(_radius * _minRandomRange, _radius * _maxRandomRange);
            _currentDelay = Random.Range(_delay * _minRandomRange, _delay * _maxRandomRange);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 0.3f);
        Vector3 previewTransform = _pivotPoint == Vector3.zero ? transform.position : _pivotPoint;
        Gizmos.DrawSphere(previewTransform, _radius);
    }
}
