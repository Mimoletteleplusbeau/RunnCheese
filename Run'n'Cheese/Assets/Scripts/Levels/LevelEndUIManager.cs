using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEndUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _timerTextMilli;

    private void Start()
    {
        LevelEndManager.Instance.OnLevelWin += ShowEndUI;
    }

    private void ShowEndUI()
    {
        TimeManager.Instance.SetTimer(_timerText, _timerTextMilli);
    }
}
