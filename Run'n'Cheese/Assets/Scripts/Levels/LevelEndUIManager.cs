using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEndUIManager : MonoBehaviour
{
    public static LevelEndUIManager Instance;
    [SerializeField] private GameObject _endUI;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _timerTextMilli;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _endUI.SetActive(false);
    }

    public void ShowEndUITransition()
    {
        Transition.Instance.SetTransition(ShowEndUI);
    }

    private void ShowEndUI()
    {
        _endUI.SetActive(true);
        TimeManager.Instance.SetTimer(_timerText, _timerTextMilli);
    }
}
