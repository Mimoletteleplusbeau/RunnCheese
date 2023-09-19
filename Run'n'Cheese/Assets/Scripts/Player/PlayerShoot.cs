using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShoot : MonoBehaviour
{
    private PlayerInputs _inputs;
    [Tooltip("The prefab of the bullet")] [SerializeField] private GameObject _prefabBullet;
    [Tooltip("The numbers of bullets available")] [SerializeField] private float _bullets;
    [Tooltip("The time between each shot in seconds")] [SerializeField] private float _reloadTime;
    private float _reloadTimer;

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
                myBullet.GetComponent<Blobfish>().Direction = mouseDirection.normalized;
            }
        }
    }
}
