using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobfish : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    public Vector2 Direction { set => _direction = value; get => _direction; }
    [HideInInspector] public GameObject Parent;
    [SerializeField] private float _speed;
    private float _destroyTime = 20f;

    [Header("Animations")]
    [SerializeField] private string _explosionAnimationName;
    [SerializeField] private float _destroyDelay;
    [Space(5)]
    [SerializeField] private GameObject _pushZonePrefab;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Destroy(gameObject, _destroyTime);
    }

    private void Update()
    {
        _rb.MovePosition(transform.position + (Vector3)Direction * _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != Parent)
        {
            SetExplosion();
        }
    }

    private void SetExplosion()
    {
        _speed = 0f;
        _animator.Play(_explosionAnimationName);
        Instantiate(_pushZonePrefab, transform.position, Quaternion.identity);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
