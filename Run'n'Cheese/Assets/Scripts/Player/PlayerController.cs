using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private PlayerInputs input;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveVector;
    private Vector2 _targetPosition;
    private Collider2D boxCollider;
    [HideInInspector] public PlayerState MyState;

    [Header("Collisions")]
    [Tooltip("The layer of the platforms the player is allowed to jump from")] [SerializeField] private LayerMask platformsLayerMask;

    [Header("Movement")]
    [Tooltip("The maximum speed the character can move at")] [SerializeField] private float maxSpeed = 1;
    [HideInInspector] public bool Flipped;
    private float speed;
    [Tooltip("The ground friction ranging from 0 to 1")] [Range(0,1)] [SerializeField] private float groundFriction = 0.1f;
    [Tooltip("The ground momentum ranging from 0 to 1")] [Range(0, 1)] [SerializeField] private float groundMomentum = 0.1f;
    [Tooltip("The air friction ranging from 0 to 1")] [Range(0, 1)] [SerializeField] private float airFriction = 0.1f;
    [Tooltip("The air momentum ranging from 0 to 1")] [Range(0, 1)] [SerializeField] private float airMomentum = 0.1f;
    private float X_Velocity;

    [Header("Jump")]
    [Tooltip("The number of jumps allowed before touching the ground")] [SerializeField] private float MaxJumps;
    private float _jumps;
    [Tooltip("The force of the jump")] [SerializeField] private float jumpForce = 1;
    [Tooltip("The minimum force of the jump")] [SerializeField] private float minJumpForce;
    private bool _jumpInput;
    private float Y_Velocity;
    [Tooltip("The gravity applied while ascending")] [SerializeField] private float fallSpeedAscent;
    [Tooltip("The gravity applied while descending")] [SerializeField] private float fallSpeedDescent;
    [Tooltip("Allows the player to slide along the walls")] [SerializeField] private bool canWallSlide;
    [Tooltip("The gravity applied while sliding along a wall")] [SerializeField] private float fallSpeedWallSliding;
    [Tooltip("The clamped fall speed")] [SerializeField] private float maxFallSpeed;
    [Tooltip("The time a jump is allowed after leaving the ground in seconds")] [SerializeField] private float coyoteTime;
    [Tooltip("The time a jump is allowed before reaching the ground in seconds")] [SerializeField] private float jumpBufferTime;
    private float _jumpBufferCounter;
    private bool _isGrounded;
    [Tooltip("The offset of the ground detection")] [SerializeField] private float _groundedOffset;

    [Header("External Forces")]
    [Tooltip("The time the player is pushed by the explosion")] [SerializeField] private float _explosionTimer;
    private float _explosionCounter;
    private Vector2 _explosionCurrentDirection;
    private Vector2 _explosionCurrentForce;
    [Tooltip("The maximum velocity the player can be launched at")][SerializeField] private float _maxVelocity;

    [Header("Special")]
    [Tooltip("Renders a trail that follows the Player's movements")] [SerializeField] private bool ShowTrail;
    private TrailRenderer _trail;
    [Tooltip("Add Squash & Strech to the sprite of the Player")] [SerializeField] private bool squashAndStrech;

    public enum PlayerState
    {
        Idle,
        Walk,
        JumpAscent,
        JumpDescent,
    }

    private void Awake()
    {
        Instance = this;
        input = new PlayerInputs();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _jumps = MaxJumps;
        boxCollider = GetComponent<Collider2D>();
        _trail = transform.GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        _targetPosition = Vector2.zero;

        CheckForGround();

        CheckForMovements();

        CheckForJumps();

        ApplyForces();

        ApplyGravity();

        ApplyPositionChanges();

        ShowSpecialEffects();

        SetStates();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementStop;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpStop;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementStop;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpStop;
    }

    #region Move
    private void OnMovementPerformed(InputAction.CallbackContext callback)
    {
        moveVector = callback.ReadValue<Vector2>();
    }

    private void OnMovementStop(InputAction.CallbackContext callback)
    {
        moveVector = Vector2.zero;
    }

    private void CheckForMovements()
    {
        float targetSpeed = moveVector.x * maxSpeed;


        //if (_wallJumpCounter > 0 && !IsGrounded()) targetSpeed = targetSpeed * ((_wallJumpCounter / wallJumpTime) * - 1 + 1) + (wallJumpDirection.x * _wallJumpCounter * _wallJumpCurrentDirection);
        float speedChangeRate = Mathf.Abs(targetSpeed) > 0.01f ? (_isGrounded ? groundFriction : airFriction) : (_isGrounded ? groundMomentum : airMomentum);
        speed = Mathf.Lerp(speed, targetSpeed, speedChangeRate);

        X_Velocity = speed;
    }
    #endregion

    #region Jump
    private void OnJumpPerformed(InputAction.CallbackContext callback)
    {
        _jumpInput = true;
    }

    private void OnJumpStop(InputAction.CallbackContext callback)
    {
        if (Y_Velocity > 0)
            EndJump();
    }

    private void CheckForJumps()
    {
        if (_jumpInput)
        {
            _jumpInput = false;
            if ((!IsTouchingWalls() || IsGrounded()) && (_explosionCounter <= 0.01f))
            {
                StartJump();
                _jumpBufferCounter = jumpBufferTime;
            }
        }
        if (_isGrounded) _jumps = MaxJumps;

        _jumpBufferCounter -= Time.deltaTime;
        _explosionCounter -= Time.deltaTime;
    }

    private void StartJump()
    {
        if (_jumps > 0)
        {
            Y_Velocity = jumpForce;
            _jumps--;
            _jumpBufferCounter = 0;
        }
    }

    private void EndJump()
    {
        Y_Velocity = Mathf.Clamp(minJumpForce, minJumpForce, Y_Velocity);
    }

    IEnumerator OnCoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        if (!IsGrounded())
            _jumps = Mathf.Clamp(_jumps, 0, MaxJumps - 1);
    }
    #endregion

    #region External Forces
    public void SetForce(Vector2 direction, Vector2 force)
    {
        _explosionCurrentDirection = direction.normalized;
        _explosionCurrentForce = force;
        _explosionCounter = _explosionTimer;
        _jumpBufferCounter = 0;
        _jumps--;
        //_jumps=0;
        //Debug.Log(_targetPosition + _explosionCurrentDirection * (_explosionCounter / _explosionTimer) * _explosionCurrentForce);
    }

    private void ApplyForces()
    {
        if (_explosionCounter > 0.01f)
        {
            float normalizedExplosionTimer = (_explosionCounter / _explosionTimer);
            _targetPosition += _explosionCurrentDirection * normalizedExplosionTimer * _explosionCurrentForce;
        }
    }
    #endregion

    #region Ground Check
    private void EnterGround()
    {
        _jumps = MaxJumps;
        _explosionCounter = 0;
        Y_Velocity = 0;
        if (_jumpBufferCounter > 0)
        {
            StartJump();
        }
        StopCoroutine(OnCoyoteTime());

        if (squashAndStrech)
            StartCoroutine(EnterGroundSquash());
    }

    private void LeaveGround()
    {
        if (_jumps == MaxJumps)
        {
            StopCoroutine(OnCoyoteTime());
            StartCoroutine(OnCoyoteTime());
        }

        if (squashAndStrech)
            StartCoroutine(LeaveGroundStretch());
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, _groundedOffset, platformsLayerMask);

        Color rayColor = Color.red;
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + _groundedOffset), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + _groundedOffset), Vector2.right * (boxCollider.bounds.extents.x), rayColor);

        return raycastHit2D.collider != null;
    }

    private void CheckForGround()
    {
        bool _isGroundedThisFrame = IsGrounded();
        if (_isGroundedThisFrame != _isGrounded)
        {
            _isGrounded = _isGroundedThisFrame;
            if (_isGroundedThisFrame)
            {
                EnterGround();
            }
            else
            {
                LeaveGround();
            }
        }
    }
    #endregion

    #region Wall Check
    private bool IsTouchingWalls()
    {
        float offset = 0.05f;
        Vector2 direction = Flipped ? Vector2.right : Vector2.left;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, direction, offset, platformsLayerMask);

        //Color rayColor = Color.red;
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y), direction * (boxCollider.bounds.extents.x + offset), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), direction * (boxCollider.bounds.extents.x + offset), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(direction.x * (boxCollider.bounds.extents.x + offset), boxCollider.bounds.extents.y), Vector2.down * (boxCollider.bounds.size.y), rayColor);

        return raycastHit2D.collider != null;
    }

    private bool IsSliding()
    {
        return IsTouchingWalls() && moveVector.x != 0;
    }
    #endregion

    #region Gravity
    private void ApplyGravity()
    {
        float _fallSpeed = (Y_Velocity >= 0 || _targetPosition.y > 0) ? fallSpeedAscent : (IsSliding() && canWallSlide) ? fallSpeedWallSliding : fallSpeedDescent;
        float _currentFallSpeed = Mathf.Clamp(Y_Velocity - _fallSpeed, -maxFallSpeed, Y_Velocity - _fallSpeed);
        if (!_isGrounded) Y_Velocity = _currentFallSpeed;
    }
    #endregion

    #region Apply Position
    private void ApplyPositionChanges()
    {
        _targetPosition += new Vector2(X_Velocity, Y_Velocity);
        _targetPosition = Vector2.ClampMagnitude(_targetPosition, _maxVelocity);
        rigidbody.MovePosition((Vector2)transform.position + _targetPosition);
    }
    #endregion

    #region States
    private void SetStates()
    {
        if (_targetPosition.x != 0)
        {
            Flipped = _targetPosition.x > 0;
        }

        if (_isGrounded)
        {
            if (Mathf.Abs(_targetPosition.x) > 0.01f)
            {
                MyState = PlayerState.Walk;
            } else
            {
                MyState = PlayerState.Idle;
            }
        } else
        {
            if (_targetPosition.y > 0)
            {
                MyState = PlayerState.JumpAscent;
            } else
            {
                MyState = PlayerState.JumpDescent;
            }
        }
    }
    #endregion

    #region SpecialEffects
    IEnumerator EnterGroundSquash()
    {
        Vector3 originalScale = transform.localScale;
        float squashTime = 0.15f;
        float normalTime = 0.2f;
        spriteRenderer.transform.DOScaleX(originalScale.x * 1.1f, squashTime);
        spriteRenderer.transform.DOScaleY(originalScale.y * 0.7f, squashTime);
        spriteRenderer.transform.DOLocalMoveY(-squashTime, squashTime);
        yield return new WaitForSeconds(squashTime);
        spriteRenderer.transform.DOScaleX(originalScale.x, normalTime);
        spriteRenderer.transform.DOScaleY(originalScale.y, normalTime);
        spriteRenderer.transform.DOLocalMoveY(0, normalTime);
        yield return new WaitForSeconds(normalTime);
    }

    IEnumerator LeaveGroundStretch()
    {
        Vector3 originalScale = transform.localScale;
        float strecthTime = 0.15f;
        float normalTime = 0.2f;
        spriteRenderer.transform.DOScaleX(originalScale.x * 0.8f, strecthTime);
        spriteRenderer.transform.DOScaleY(originalScale.y * 1.2f, strecthTime);
        yield return new WaitForSeconds(strecthTime);
        spriteRenderer.transform.DOScaleX(originalScale.x, normalTime);
        spriteRenderer.transform.DOScaleY(originalScale.y, normalTime);
        yield return new WaitForSeconds(normalTime);
    }

    private void ShowSpecialEffects()
    {
        _trail.gameObject.SetActive(ShowTrail);
    }
    #endregion

}