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
    private float _checkTimer;
    private float _cooldownFrames = 2f;

    [field:SerializeField] protected override GameObject _VFXDeath { get; set; }


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
        _checkTimer--;
        if (_grounded && (!IsGrounded() || IsTouchingWalls()) && _checkTimer < 0)
        {
            _direction *= -1;
            _grounded = false;
            _checkTimer = _cooldownFrames;
        }
        else if (IsGrounded() || !IsTouchingWalls())
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

    private bool IsTouchingWalls()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, _direction, _speed, platformsLayerMask);

        //Color rayColor = Color.red;
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y), direction * (boxCollider.bounds.extents.x + offset), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), direction * (boxCollider.bounds.extents.x + offset), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(direction.x * (boxCollider.bounds.extents.x + offset), boxCollider.bounds.extents.y), Vector2.down * (boxCollider.bounds.size.y), rayColor);

        return raycastHit2D.collider != null;
    }

    private void OnDestroy()
    {
        SpawnDeathVFX();
    }
}
