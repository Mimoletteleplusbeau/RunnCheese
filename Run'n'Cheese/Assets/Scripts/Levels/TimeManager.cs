using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private float _timer;
    [SerializeField] private TMP_Text _timerText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _timerText.text = Mathf.Floor(_timer).ToString();
    }
}
