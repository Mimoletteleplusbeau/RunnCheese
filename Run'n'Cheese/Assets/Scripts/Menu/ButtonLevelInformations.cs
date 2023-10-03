using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLevelInformations : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject[] _activeFishes;

    private void Start()
    {
        LevelData myLevel = LevelsManager.Instance.GetLevelList()[ID];

        if (myLevel.BestTime == int.MaxValue)
        {
            _timerText.text = "--,--s";
        } else
        {
            float seconds = Mathf.Floor(myLevel.BestTime % 60);
            float milliseconds = myLevel.BestTime % 1 * 100;
            _timerText.text = string.Format("{0:00},{1:00}", seconds, milliseconds) + "s";
        }

        for (int i = 0; i < myLevel.HasFishes.Length; i++)
        {
            bool hasFish = myLevel.HasFishes[i];
            _activeFishes[i].SetActive(hasFish);
        }
    }
}
