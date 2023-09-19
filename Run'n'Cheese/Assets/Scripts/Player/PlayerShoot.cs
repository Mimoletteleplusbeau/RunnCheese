using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Tooltip("The prefab of the bullet")] [SerializeField] private GameObject _prefabBullet;
    [Tooltip("The numbers of bullets available")] [SerializeField] private float _bullets;
    [Tooltip("The time between each shot in seconds")] [SerializeField] private float _reloadTime;
    private float _reloadTimer;

    private void Update()
    {
        _reloadTimer -= Time.deltaTime;
    }

    private void OnShoot()
    {
        if (_bullets > 0 && _reloadTimer < 0)
        {
            _reloadTimer = _reloadTime;
            _bullets--;
            var myBullet = Instantiate(_prefabBullet);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
