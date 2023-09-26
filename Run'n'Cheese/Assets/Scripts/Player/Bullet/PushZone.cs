using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{
    private CircleCollider2D _collider;
    [Tooltip("The force applied by the push force")][SerializeField] private Vector2 _force;
    [Tooltip("The radius ofthe push zone")][SerializeField] private float _radius;
    [Tooltip("The time the push zone stays active in seconds")] [SerializeField] private float _activationTime;
    [HideInInspector] public Vector2 HitNormal;
    private bool _hasHitPlayer = false;

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
        if (_hasHitPlayer) return;

        float distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distance <= _radius)
        {
            Vector2 playerDirection = HitNormal;
            if (Mathf.Abs(playerDirection.x) > 0)
            {
                if (PlayerController.Instance.MyState == PlayerController.PlayerState.JumpAscent || PlayerController.Instance.MyState == PlayerController.PlayerState.JumpDescent)
                {
                    playerDirection.y = 1;
                    //if (transform.position.y < PlayerController.Instance.transform.position.y)
                    //{
                    //    playerDirection.x = 0;
                    //}
                }
            }
            PlayerController.Instance.SetForce(playerDirection, _force);
            ScreenShake.Instance.Shake(_explosionShakeForce, _explosionShakeTime);
            _hasHitPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
