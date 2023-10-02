using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsList : MonoBehaviour
{

    static public SoundsList Instance;

    [Header("Player")]
    [SerializeField] private AudioClip _playerJump;
    [SerializeField] private AudioClip _playerExplosionEjection;
    [SerializeField] private AudioClip _playerDeath;

    [Header("Menu")]
    [SerializeField] private AudioClip _buttonClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += ReloadLevel;
        LoadLevel();
    }

    private void ReloadLevel(Scene arg0, LoadSceneMode arg1)
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        PlayerController.Instance.OnJump += PlayPlayerJump;
        PlayerController.Instance.OnExplosionEjection += PlayPlayerExplosionEjection;
        PlayerController.Instance.OnDeath += PlayPlayerDeath;
    }

    void PlayPlayerJump() => PlaySound(_playerJump);
    void PlayPlayerExplosionEjection() => PlaySound(_playerExplosionEjection);
    void PlayPlayerDeath() => PlaySound(_playerDeath);
    public void PlayButtonClick() => PlaySound(_buttonClick);

    private void PlaySound(AudioClip clip)
    {
        SoundManager.Instance.PlaySound(clip);
    }
}
