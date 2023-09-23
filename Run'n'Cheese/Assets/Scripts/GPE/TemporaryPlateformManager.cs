using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlateformManager : MonoBehaviour
{
    [SerializeField] private TemporaryPlateform _tempPlateformPrefab;
    [SerializeField] private TemporaryPlateform _startPlateform;
    private Vector2 _plateformPosition;
    private Quaternion _plateformRotation;
    private Vector2 _plateformScale;
    private float _plateformRadius;
    [SerializeField] private float _respawnTime;
    private bool _canSpawn;
    private float _playerDistanceOffset = 1.05f;

    private void Start()
    {
        SaveTransform(_startPlateform.transform);
        _startPlateform.OnDeath += CreateNewPlateform;
    }

    private void Update()
    {
        if (!_canSpawn) return;

        CheckForPlayer();
    }

    private void CreateNewPlateform()
    {
        StartCoroutine(WaitForRespawnTime());
    }

    IEnumerator WaitForRespawnTime()
    {
        yield return new WaitForSeconds(_respawnTime);
        _canSpawn = true;
    }

    private void SpawnNewPlateform()
    {
        _canSpawn = false;
        var newPlateform = Instantiate(_tempPlateformPrefab, transform);
        SetTransform(newPlateform.transform);
        newPlateform.OnDeath += CreateNewPlateform;
    }

    private void CheckForPlayer()
    {
        if (Vector2.Distance(_plateformPosition, PlayerController.Instance.transform.position) > _plateformRadius * _playerDistanceOffset)
        {
            SpawnNewPlateform();
        }
    }

    private void SaveTransform(Transform transform)
    {
        _plateformPosition = transform.transform.position;
        _plateformRotation = transform.transform.rotation;
        _plateformScale = transform.transform.localScale;
        Vector2 plateformBounds = transform.GetComponent<Collider2D>().bounds.extents;
        _plateformRadius = Mathf.Max(plateformBounds.x, plateformBounds.y);
    }

    private void SetTransform(Transform transform)
    {
        transform.position = _plateformPosition;
        transform.rotation = _plateformRotation;
        transform.localScale = _plateformScale;
    }
}