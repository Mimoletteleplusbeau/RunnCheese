using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{
    [Tooltip("The force applied by the push force")][SerializeField] private float _force;
    [Tooltip("The radius ofthe push zone")][SerializeField] private float _radius;
    [Tooltip("The time the push zone stays active in seconds")] [SerializeField] private float _activationTime;
    private Collider2D[] _collidersGround;

    private void Start()
    {
        _collidersGround = new Collider2D[1];
        Destroy(gameObject, _activationTime);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distance <= _radius)
        {
            Vector3 playerDirection = PlayerController.Instance.transform.position - transform.position;
            PlayerController.Instance.SetForce(playerDirection, _force);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
