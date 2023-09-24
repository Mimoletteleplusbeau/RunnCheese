using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private float _timer;
    private bool _hasTimerStopped;
    [SerializeField] private TMP_Text _timerText;

    private void Awake()
    {
        instance = this;
        Seagull.Instance.OnKill += StopTimer;
    }

    private void Update()
    {
        if (_hasTimerStopped) return;

        _timer += Time.deltaTime;
        _timerText.text = Mathf.Floor(_timer).ToString();
    }

    private void StopTimer()
    {
        _hasTimerStopped = true;
    }
}
