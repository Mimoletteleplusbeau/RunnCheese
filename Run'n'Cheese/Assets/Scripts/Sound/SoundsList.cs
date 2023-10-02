using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsList : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip _playerJump;


    private void Start()
    {
        PlayerController.Instance.OnJump += PlayPlayerJump;
    }

    void PlayPlayerJump() => PlaySound(_playerJump);

    private void PlaySound(AudioClip clip)
    {
        SoundManager.Instance.PlaySound(clip);
    }
}
