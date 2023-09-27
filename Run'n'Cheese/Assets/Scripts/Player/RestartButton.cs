using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RestartButton : MonoBehaviour
{
    private PlayerInputs _playerinput;

    private void Awake()
    {
        _playerinput = new PlayerInputs();
    }

    private void OnEnable()
    {
        _playerinput.Enable();
        _playerinput.Player.Restart.performed += OnRestart;
    }

    private void OnDisable()
    {
        _playerinput.Disable();
        _playerinput.Player.Restart.performed -= OnRestart;
    }

    private void OnRestart(InputAction.CallbackContext callback)
    {
        LevelsManager.Instance.RestartLevel();
    }


}
