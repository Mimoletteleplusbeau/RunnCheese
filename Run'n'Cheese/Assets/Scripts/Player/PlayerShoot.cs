using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class PlayerShoot : MonoBehaviour
{
    public Action OnPlayerShoot;

    private PlayerInputs _inputs;
    [Tooltip("The prefab of the bullet")] [SerializeField] private GameObject _prefabBullet;
    private float _bullets;
    [Tooltip("The numbers of bullets available")] [SerializeField] private float _bulletsMax;
    [Tooltip("The time between each shot in seconds")] [SerializeField] private float _reloadTime;
    private float _reloadTimer;

    [Header("Gun Animation")]
    [Tooltip("The Sprite of the gun")] [SerializeField] private SpriteRenderer _spriteGun;
    [Tooltip("The Animator of the gun")] [SerializeField] private Animator _spriteGunAnimator;
    [Tooltip("The sprite of the player")] [SerializeField] private SpriteRenderer _sprite;
    [Tooltip("The name of the animation of the gun firing")] [SerializeField] private string _gunAnimationName;

    private void Awake()
    {
        _inputs = new PlayerInputs();
        _bullets = _bulletsMax;
    }



    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Shoot.performed -= OnShoot;
    }

    private void Update()
    {
        if (!PlayerController.Instance.CanMove) return;

        _reloadTimer -= Time.deltaTime;
        TurnGunAnimation();
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController.MyState == PlayerController.PlayerState.JumpAscent || playerController.MyState == PlayerController.PlayerState.JumpDescent)
        {
            return;
        }
        Reload();
    }

    private void TurnGunAnimation()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = Camera.main.WorldToScreenPoint((Vector2)_spriteGun.transform.position);
        Vector2 mouseDirection = mousePosition - playerPosition;
        _spriteGun.flipX = _sprite.flipX;
        int flipDirection = (_spriteGun.flipX ? -1 : 1);
        _spriteGun.transform.right = mouseDirection * flipDirection;
        _spriteGun.transform.localPosition = new Vector3(Mathf.Abs(_spriteGun.transform.localPosition.x) * -flipDirection, _spriteGun.transform.localPosition.y);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (_bullets > 0 && _reloadTimer < 0)
        {
            OnPlayerShoot?.Invoke();
            
            _reloadTimer = _reloadTime;
            _bullets--;
            GameObject newBullet = Instantiate(_prefabBullet, transform.position, Quaternion.identity);
            Bullet myBullet = newBullet.GetComponent<Bullet>();
            Vector2 mousePosition = Input.mousePosition;
            Vector2 playerPosition = Camera.main.WorldToScreenPoint((Vector2)_spriteGun.transform.position);
            Vector2 mouseDirection = mousePosition - playerPosition;
            if (myBullet != null)
            {
                myBullet.Direction = mouseDirection.normalized;
                myBullet.Parent = this.gameObject;
            }
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            Vector2 originalGunPosition = _spriteGun.transform.localPosition;
            float _recoilOffset = 2f;
            sequence.Append(_spriteGun.transform.DOLocalMove(new Vector2(originalGunPosition.x + mouseDirection.normalized.x * _recoilOffset, originalGunPosition.y - mouseDirection.normalized.y * _recoilOffset), 0.05f));
            sequence.Append(_spriteGun.transform.DOLocalMove(originalGunPosition, 0.2f));
            sequence.Play();

            _spriteGunAnimator.Play(_gunAnimationName);
        }
    }
    public void Reload()
    {
        _bullets = _bulletsMax;
    }
}
