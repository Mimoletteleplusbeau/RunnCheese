using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private bool _hasPlayerMoved;
    private float _timer;
    private bool _hasTimerStopped;
    [SerializeField] private TMP_Text _timerText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Seagull.Instance.OnKill += StopTimer;
        PlayerController.Instance.OnStateChange += CheckForPlayerInputs;

        _timerText.text = "00:00";
    }

    private void Update()
    {
        if (!_hasPlayerMoved) return;
        if (_hasTimerStopped) return;

        _timer += Time.deltaTime;
        float minutes = Mathf.Floor(_timer/60);
        float seconds = Mathf.Floor(_timer%60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void StopTimer()
    {
        _hasTimerStopped = true;
    }

    private void CheckForPlayerInputs()
    {
        if (PlayerController.Instance.MyState == PlayerController.PlayerState.Walk || PlayerController.Instance.MyState == PlayerController.PlayerState.JumpAscent)
            _hasPlayerMoved = true;
    }
}
