using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForces : MonoBehaviour
{
    public static PlayerForces Instance;
    private Vector2 _currentDirection;
    private float _currentForce;
    [SerializeField] private float _forceDiminution;
    private Rigidbody2D _rb;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetForce(Vector2 direction, float force)
    {
        _currentDirection = direction;
        _currentForce = force;
        _rb.MovePosition((Vector2)transform.position + direction * force);
    }
}
