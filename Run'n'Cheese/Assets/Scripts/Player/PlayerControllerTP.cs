using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;

public class PlayerControllerTP : MonoBehaviour
{
    private Vector2 _inputs;
    private bool _inputJump;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    [Header("Movements")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _acceleration;

    [Header("Ground Check")]
    [SerializeField] private float _groundOffset;
    [SerializeField] private float _groundRadius;
    //private Collider2D[] _collidersGround;
    private RaycastHit2D[] _groundHitResults;
    [SerializeField] private LayerMask _GroundLayer;
    private float _timeSinceGrounded;
    [SerializeField] private float _coyoteTime;

    [Header("Jump")]
    private bool _isGrounded;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _timeMinBetweenJump;
    private float _timerNoJump;
    [SerializeField] private float _velocityFallMin;
    [Tooltip("Gravity applied in most cases")] [SerializeField] private float _gravity;
    [Tooltip("Gravity applied when jumping")][SerializeField] private float _gravityUpJump;
    private float _timeSinceJumpPressed;
    [SerializeField] private float _jumpInputTimer;

    //[Header("Slope Check")]
    //[SerializeField] private float _slopeDetectOffset;
    //private RaycastHit2D[] _hitResults;
    //private bool _isOnSlope;
    //[SerializeField] private PhysicsMaterial2D _physicsFriction;
    //[SerializeField] private PhysicsMaterial2D _physicsNoFriction;

    //[Header("Corner Check")]
    //[SerializeField] private float[] directions;
    //[SerializeField] private Vector2 _offsetCollisionBox;
    //[SerializeField] private Vector2 _offsetToReplace;
    //[SerializeField] private Vector2 _collisionBox;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _groundHitResults = new RaycastHit2D[2];
        //_collidersGround = new Collider2D[2];
        //_hitResults = new RaycastHit2D[2];
    }

    private void Update()
    {
        HandleInputs();
        HandleGround();
        //HandleSlope();
        //HandleCorners();
    }

    private void FixedUpdate()
    {
        HandleMovements();
        HandleJump();

    }


    private void HandleInputs()
    {
        _inputs.x = Input.GetAxisRaw("Horizontal");
        _inputs.y = Input.GetAxisRaw("Vertical");
        _inputJump = Input.GetKey(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _timeSinceJumpPressed = 0;
        }
    }

    private void HandleMovements()
    {
        var velocity = _rb.velocity;
        Vector2 wantedVelocity = new Vector2(_inputs.x * _walkSpeed, velocity.y);
        _rb.velocity = Vector2.MoveTowards(velocity, wantedVelocity, _acceleration * Time.deltaTime);
    }


    private void HandleGround()
    {
        _timeSinceGrounded += Time.deltaTime;

        //Vector2 point = transform.position + Vector3.up * _groundOffset;
        //bool currentGrounded = Physics2D.OverlapCircleNonAlloc(point, _groundRadius, _collidersGround, _GroundLayer) > 0;

        //int results = Physics2D.BoxCastNonAlloc(_collider.bounds.center, _collider.bounds.extents, 0f, Vector2.down, _groundHitResults, _groundOffset, _GroundLayer);
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.extents, 0f, Vector2.down, _groundOffset, _GroundLayer);

        //Color rayColor = Color.red;
        //Debug.DrawRay(_collider.bounds.center + new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + _groundOffset), rayColor);
        //Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, 0), Vector2.down * (_collider.bounds.extents.y + _groundOffset), rayColor);
        //Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, _collider.bounds.extents.y + _groundOffset), Vector2.right * (_collider.bounds.extents.x), rayColor);
        bool currentGrounded = raycastHit2D.collider != null;
        Debug.Log(currentGrounded);
        //Debug.DrawLine(point, new Vector3(point.x, point.y - _groundRadius), Color.red);

        if (!currentGrounded && _isGrounded)
        {
            _timeSinceGrounded = 0;
        }
        _isGrounded = currentGrounded;
    }

    private void HandleJump()
    {
        _timerNoJump -= Time.deltaTime;
        _timeSinceJumpPressed += Time.deltaTime;

        if (_inputJump && _rb.velocity.y <= 0 && (_isGrounded || _timeSinceGrounded < _coyoteTime)
            && _timerNoJump <= 0 && _timeSinceJumpPressed < _jumpInputTimer)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _timerNoJump = _timeMinBetweenJump;
        }

        if (_rb.velocity.y < _velocityFallMin)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _velocityFallMin);
        }

        if (!_isGrounded)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.gravityScale = _gravity;
            } else
            {
                _rb.gravityScale = _inputJump ? _gravityUpJump : _gravity;
            }
        } else
        {
            _rb.gravityScale = _gravity;
        }
    }

    //private void HandleSlope()
    //{
    //    Vector3 origin = _collider.bounds.min + Vector3.right * _collider.bounds.size.x;
    //    bool slopeRight = Physics2D.RaycastNonAlloc(origin, Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;
    //    bool slopeLeft = Physics2D.RaycastNonAlloc(origin, -Vector2.right, _hitResults, _slopeDetectOffset, _GroundLayer) > 0;
    //    //Debug.DrawLine(origin, origin + Vector3.right * _slopeDetectOffset, Color.red);
    //    _isOnSlope = (slopeRight || slopeLeft) && (!slopeRight || !slopeLeft);
    //    if (Mathf.Abs(_inputs.x) < 0.01f && (slopeLeft || slopeRight))
    //    {
    //        _collider.sharedMaterial = _physicsFriction;
    //    } else
    //    {
    //        _collider.sharedMaterial = _physicsNoFriction;
    //    }
    //}

    //private void HandleCorners()
    //{
    //    for (int i = 0; i < directions.Length; i++)
    //    {
    //        float dir = directions[i];

    //        if (Mathf.Abs(_inputs.x) > 0.1f && Mathf.Abs(Mathf.Sign(dir) - Mathf.Sign(_inputs.x)) < 0.001f
    //            && !_isGrounded && !_isOnSlope)
    //        {
    //            Vector3 position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x,
    //                _offsetCollisionBox.y, 0);
    //            Debug.DrawLine(position, position * 2);

    //            int result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero,
    //                _hitResults, 0, _GroundLayer);

    //            if (result > 0)
    //            {
    //                position = transform.position + new Vector3(_offsetCollisionBox.x + dir * _offsetToReplace.x,
    //                    _offsetCollisionBox.y + _offsetToReplace.y, 0);

    //                result = Physics2D.BoxCastNonAlloc(position, _collisionBox, 0, Vector2.zero,
    //                _hitResults, 0, _GroundLayer);

    //                if (result == 0)
    //                {
    //                    Debug.Log("Replace");
    //                    transform.position += new Vector3(dir * _offsetToReplace.x, _offsetToReplace.y);

    //                    if (_rb.velocity.y < 0)
    //                    {
    //                        _rb.velocity = new Vector2(_rb.velocity.x, 0);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
