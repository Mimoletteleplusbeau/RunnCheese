using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    [SerializeField] private float _speed;
    private Vector3 _direction;
    private Collider2D _boxCollider;

    [Header("Collisions")]
    [Tooltip("The layer of the platforms the enemy can detect")] [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private float _groundedOffset = 0.05f;
    private float _checkTime;

    private void Awake()
    {
        _direction = Vector2.right;
        _boxCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        transform.position += _direction * _speed;
        CheckForGround();
    }

    private void CheckForGround()
    {
        _checkTime -= Time.deltaTime;
        if (_checkTime > 0) return;
        if (!IsGrounded())
        {
            _direction *= -1;
            _checkTime = 1;
        } else
        {
            Debug.Log("grounded");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, _groundedOffset, platformsLayerMask);

        Color rayColor = Color.red;
        Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, _boxCollider.bounds.extents.y + _groundedOffset), Vector2.right * (_boxCollider.bounds.extents.x), rayColor);

        return raycastHit2D.collider != null;
    }
}
