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
    [SerializeField] private float _respawnTime;
    private bool _canSpawn;
    private float _playerDistance = 2f;

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
        Debug.Log("check for player");
        if (Vector2.Distance(_plateformPosition, (Vector2)PlayerController.Instance.transform.position) > _playerDistance)
        {
            SpawnNewPlateform();
        }
    }

    private void SaveTransform(Transform transform)
    {
        _plateformPosition = transform.transform.position;
        _plateformRotation = transform.transform.rotation;
        _plateformScale = transform.transform.localScale;
    }

    private void SetTransform(Transform transform)
    {
        transform.position = _plateformPosition;
        transform.rotation = _plateformRotation;
        transform.localScale = _plateformScale;
    }
}