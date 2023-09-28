using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class RestartAndQuitButtons : MonoBehaviour
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
        _playerinput.Player.Quit.performed += OnQuit;
    }

    private void OnDisable()
    {
        _playerinput.Disable();
        _playerinput.Player.Restart.performed -= OnRestart;
        _playerinput.Player.Quit.performed -= OnQuit;
    }

    private void OnRestart(InputAction.CallbackContext callback)
    {
        LevelsManager.Instance.RestartLevel();
    }

    private void OnQuit(InputAction.CallbackContext callback)
    {
        LevelsManager.Instance.GoToMenu();
    }
}
