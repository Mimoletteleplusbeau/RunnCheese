using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCutsceneManager : MonoBehaviour
{
    static public BeginningCutsceneManager Instance;

    [SerializeField] private GameObject _camera;
    [SerializeField] private float _cutsceneTime;
    [SerializeField] private float _laughTime;

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
        yield return new WaitForSeconds(_laughTime);
        if (Seagull.Instance != null)  SoundManager.Instance.PlaySound(Seagull.Instance.SFXLaugh);
        yield return new WaitForSeconds(_cutsceneTime - _laughTime);
        DisableCamera();
    }

    private void DisableCamera()
    {
        _camera.SetActive(false);
        PlayerController.Instance.SetMovable(true);
    }
}
