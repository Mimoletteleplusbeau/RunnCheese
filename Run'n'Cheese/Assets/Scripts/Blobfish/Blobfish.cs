using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blobfish : MonoBehaviour
{

    [Tooltip("The layer with which the bullet")] [SerializeField] private LayerMask _collidingLayer;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    public Vector2 Direction { set => _direction = value; get => _direction; }
    [SerializeField] private float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rb.MovePosition(transform.position + (Vector3)Direction * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.gameObject.layer == _collidingLayer)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}
