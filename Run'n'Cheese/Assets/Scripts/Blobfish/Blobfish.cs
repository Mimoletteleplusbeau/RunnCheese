using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Blobfish : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    public Vector2 Direction { set => _direction = value; get => _direction; }
    [HideInInspector] public GameObject Parent;
    [Tooltip("The speed the bullet travells at")] [SerializeField] private float _speed;
    private float _destroyTime = 20f;
    private bool _isSticked;
    [SerializeField] private LayerMask _defaultLayer;

    [Header("Animations")]
    [Tooltip("The name of the explosion animation")] [SerializeField] private string _explosionAnimationName;
    [Space(5)]
    [Tooltip("The prefab of the push zone")] [SerializeField] private GameObject _pushZonePrefab;

    [Header("FX")]
    [SerializeField] private GameObject _explosionVFXPrefab;
    [SerializeField] private float _explosionShakeForce;
    [SerializeField] private float _explosionShakeTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Invoke("DestroySelf", _destroyTime);
    }

    private void Update()
    {
        _rb.MovePosition(transform.position + (Vector3)Direction * _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != Parent)
        {
            if (collision.gameObject.GetComponent<Sticky>() != null)
            {
                SetStick();

            } else {

                SetExplosion();
            }
        }
    }

    private void SetExplosion()
    {
        _speed = 0f;
        _animator.Play(_explosionAnimationName);
        
        Instantiate(_pushZonePrefab, transform.position, Quaternion.identity);

        // Feedback
        Instantiate(_explosionVFXPrefab, transform.position, Quaternion.identity);
        ScreenShake.Instance.Shake(_explosionShakeForce, _explosionShakeTime);
    }

    private void SetStick()
    {
        _speed = 0;
        CancelInvoke("DestroySelf");
        _isSticked = true;
        gameObject.layer = _defaultLayer;
        Parent = this.gameObject;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
