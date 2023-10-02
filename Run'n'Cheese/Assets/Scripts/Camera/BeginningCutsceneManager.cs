using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCutsceneManager : MonoBehaviour
{
    static public BeginningCutsceneManager Instance;

    [SerializeField] private GameObject _camera;
    [SerializeField] private float _cutsceneTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _camera.SetActive(true);
        StartCoroutine(StartCutsceneTimer());
    }

    IEnumerator StartCutsceneTimer()
    {
        yield return new WaitForSeconds(_cutsceneTime);
        DisableCamera();
    }

    private void DisableCamera()
    {
        _camera.SetActive(false);
        PlayerController.Instance.SetMovable(true);
    }
}
