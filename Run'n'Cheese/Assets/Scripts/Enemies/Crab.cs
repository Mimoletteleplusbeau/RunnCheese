using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    [SerializeField] private float _speed;
    private Vector3 _direction;
    private Collider2D _boxCollider;
    private Rigidbody2D _rb;
    private RaycastHit2D[] _raycastHits;

    [Header("Collisions")]
    [Tooltip("The layer of the platforms the enemy can detect")] [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private float _groundedOffset = 0.05f;
    private bool _grounded;

    private void Awake()
    {
        _direction = Vector2.right;
        _boxCollider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _raycastHits = new RaycastHit2D[2];
    }

    private void Update()
    {
        Move();
        CheckForGround();
    }

    private void Move()
    {
        Vector2 targetPosition = transform.position + _direction * _speed;
        _rb.MovePosition(targetPosition);
    }

    private void CheckForGround()
    {
        Debug.Log(IsGrounded());
        if (_grounded && !IsGrounded())
        {
            _direction *= -1;
            _grounded = false;
        }
        else if (IsGrounded())
        {
            _grounded = true;
        }
    }

    private bool IsGrounded()
    {
        int results = Physics2D.RaycastNonAlloc(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x * _direction.x, 0), Vector2.down, _raycastHits, _boxCollider.bounds.extents.y + _groundedOffset, platformsLayerMask);

        Color rayColor = Color.red;
        Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, _boxCollider.bounds.extents.y + _groundedOffset), Vector2.right * (_boxCollider.bounds.extents.x), rayColor);

        return results > 0;
    }
}
