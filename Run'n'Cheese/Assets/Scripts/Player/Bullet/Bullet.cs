using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bullet : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Vector2 _direction;
    public Vector2 Direction { set => _direction = value; get => _direction; }
    [HideInInspector] public GameObject Parent;
    [Tooltip("The speed the bullet travells at")] [SerializeField] private float _speed;
    private float _destroyTime = 20f;
    private bool _isSticked;
    [SerializeField] private LayerMask _defaultLayer;
    private Vector2 _hitNormal;

    [Space(5)]
    [Tooltip("The prefab of the push zone")] [SerializeField] private PushZone _pushZonePrefab;

    [Header("FX")]
    [SerializeField] private GameObject _explosionVFXPrefab;
    [SerializeField] private float _explosionShakeForce;
    [SerializeField] private float _explosionShakeTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartDestroy());
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
                SetStick(collision);

            } else {

                SetExplosion(collision);
            }
        }
    }

    private void SetExplosion(Collision2D collision)
    {
        _speed = 0f;

        if (!_isSticked)
            _hitNormal = collision.contacts[0].normal;
        PushZone myPushZone = Instantiate(_pushZonePrefab, transform.position, Quaternion.identity);
        myPushZone.HitNormal = _hitNormal;

        // Feedback
        Instantiate(_explosionVFXPrefab, transform.position, Quaternion.identity);
        ScreenShake.Instance.Shake(_explosionShakeForce, _explosionShakeTime);

        Destroy(gameObject);
    }

    private void SetStick(Collision2D collision)
    {
        _isSticked = true;
        _hitNormal = collision.contacts[0].normal;
        _speed = 0;
        StopCoroutine(StartDestroy());
        gameObject.layer = _defaultLayer;
        Parent = this.gameObject;
    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }
}
