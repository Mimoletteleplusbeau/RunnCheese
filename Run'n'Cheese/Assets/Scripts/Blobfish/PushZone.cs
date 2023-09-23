using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{
    private CircleCollider2D _collider;
    [Tooltip("The force applied by the push force")][SerializeField] private Vector2 _force;
    [Tooltip("The radius ofthe push zone")][SerializeField] private float _radius;
    [Tooltip("The time the push zone stays active in seconds")] [SerializeField] private float _activationTime;

    [Header("FX")]
    [SerializeField] private float _explosionShakeForce;
    [SerializeField] private float _explosionShakeTime;

    private void Start()
    {
        Destroy(gameObject, _activationTime);
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = _radius;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distance <= _radius)
        {
            Vector3 playerDirection = PlayerController.Instance.transform.position - transform.position;
            Vector3 playerInputDirection = PlayerController.Instance.MoveVector;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerDirection = (playerInputDirection + playerDirection) / 2;
            if (PlayerController.Instance.transform.position.x - mousePosition.x > 5)
            {
                Debug.Log("hihihi");
            }
            PlayerController.Instance.SetForce(playerDirection, _force);
            ScreenShake.Instance.Shake(_explosionShakeForce, _explosionShakeTime);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
