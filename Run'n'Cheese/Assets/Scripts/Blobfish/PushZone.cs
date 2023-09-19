using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _radius;
    [SerializeField] private float _activationTime;
    private Collider2D[] _collidersGround;

    private void Start()
    {
        _collidersGround = new Collider2D[1];
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerForces.Instance.transform.position);
        if (distance <= _radius)
        {
            Vector3 playerDirection = PlayerForces.Instance.transform.position - transform.position;
            PlayerForces.Instance.SetForce(playerDirection, _force);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
