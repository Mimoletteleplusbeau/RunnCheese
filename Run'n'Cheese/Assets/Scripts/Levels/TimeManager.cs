using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    private bool _hasPlayerMoved;
    private float _timer;
    private bool _hasTimerStopped;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _timerTextMilli;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Seagull.Instance.OnKill += StopTimer;
        PlayerController.Instance.OnStateChange += CheckForPlayerInputs;

        _timerText.text = "00:00";
        _timerTextMilli.text = "00";
    }

    private void Update()
    {
        if (!_hasPlayerMoved) return;
        if (_hasTimerStopped) return;

        _timer += Time.deltaTime;
        SetTimer(_timerText, _timerTextMilli);
    }

    public void SetTimer(TMP_Text _timerSeconds, TMP_Text _timerMilliseconds)
    {
        float minutes = Mathf.Floor(_timer / 60);
        float seconds = Mathf.Floor(_timer % 60);
        float milliseconds = _timer % 1 * 100;
        _timerSeconds.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        _timerMilliseconds.text = string.Format("{0:00}", milliseconds);
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
