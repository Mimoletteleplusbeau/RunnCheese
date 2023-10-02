using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsList : MonoBehaviour
{

    static public SoundsList Instance;

    [Header("Clips")]
    [SerializeField] private AudioClip _playerJump;
    [SerializeField] private AudioClip _playerExplosionEjection;
    [SerializeField] private AudioClip _playerDeath;

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
        PlayerController.Instance.OnJump += PlayPlayerJump;
        PlayerController.Instance.OnExplosionEjection += PlayPlayerExplosionEjection;
        PlayerController.Instance.OnDeath += PlayPlayerDeath;
    }

    void PlayPlayerJump() => PlaySound(_playerJump);
    void PlayPlayerExplosionEjection() => PlaySound(_playerExplosionEjection);
    void PlayPlayerDeath() => PlaySound(_playerDeath);

    private void PlaySound(AudioClip clip)
    {
        SoundManager.Instance.PlaySound(clip);
    }
}
