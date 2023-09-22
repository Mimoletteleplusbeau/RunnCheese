using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


public class PlayerShoot : MonoBehaviour
{
    private PlayerInputs _inputs;
    [Tooltip("The prefab of the bullet")] [SerializeField] private GameObject _prefabBullet;
    [Tooltip("The numbers of bullets available")] [SerializeField] private float _bullets;
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
        _reloadTimer -= Time.deltaTime;
        TurnGunAnimation();
    }

    private void TurnGunAnimation()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = mousePosition - (Vector2)_spriteGun.transform.position;
        _spriteGun.flipX = _sprite.flipX;
        int flipDirection = (_spriteGun.flipX ? -1 : 1);
        _spriteGun.transform.right = mouseDirection * flipDirection;
        _spriteGun.transform.localPosition = new Vector3(Mathf.Abs(_spriteGun.transform.localPosition.x) * -flipDirection, _spriteGun.transform.localPosition.y);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (_bullets > 0 && _reloadTimer < 0)
        {
            _reloadTimer = _reloadTime;
            _bullets--;
            GameObject myBullet = Instantiate(_prefabBullet, transform.position, Quaternion.identity);
            Blobfish myBlobfish = myBullet.GetComponent<Blobfish>();
            if (myBlobfish != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseDirection = mousePosition - (Vector2)transform.position;
                myBlobfish.Direction = mouseDirection.normalized;
                myBlobfish.Parent = this.gameObject;
            }

            Sequence sequence = DOTween.Sequence();
            Vector2 originalGunPosition = _spriteGun.transform.localPosition;
            int flipDirection = (_spriteGun.flipX ? -1 : 1);
            sequence.Append(_spriteGun.transform.DOLocalMove(originalGunPosition + (Vector2)_spriteGun.transform.right * 2f * flipDirection, 0.05f));
            sequence.Append(_spriteGun.transform.DOLocalMove(originalGunPosition, 0.2f));
            sequence.Play();
            Debug.Log(_spriteGun.transform.right);

            _spriteGunAnimator.Play(_gunAnimationName);
        }
    }
}
