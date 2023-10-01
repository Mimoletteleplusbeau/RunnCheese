using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LevelEndUIManager : MonoBehaviour
{
    public static LevelEndUIManager Instance;
    [SerializeField] private GameObject _endUI;

    [Header("Timer")]
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _timerTextMilli;
    [SerializeField] private TMP_Text _timeLeft;
    [Tooltip("The sentence that appears when the high score hasn't been beaten with '{0}' to include the timer")] [SerializeField] private string _TimeToBeatSentence;
    [Tooltip("The sentence that appears when the high score has been beaten")] [SerializeField] private string _AllFishesSentence;

    [Header("Stars")]
    [SerializeField] private GameObject[] _fishTimers;

    [Header("DOTween")]
    [SerializeField] private float _starStartScale = 20;
    [SerializeField] private float _timeBeforeStarsApparition = 0.8f;
    [SerializeField] private float _timeBetweenStars = 0.7f;
    [SerializeField] private float _starApparitionTime = 0.4f;



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

        float[] fishTimes = LevelsManager.Instance.GetFishTimer();
        float finalTime = TimeManager.Instance.GetTimer();
        bool _hasAllFishes = true;

        for (int i = 0; i < fishTimes.Length; i++)
        {
            if (fishTimes[i] >= finalTime)
            {
                _fishTimers[i].SetActive(true);
                _fishTimers[i].transform.localScale = Vector2.zero;
            } else
            {
                float timeDifference = finalTime - fishTimes[i];
                timeDifference = Mathf.Round(timeDifference * 100) / 100;
                string[] timeToBeatSentences = _TimeToBeatSentence.Split("{0}");
                string preffix = timeToBeatSentences[0];
                string suffix = timeToBeatSentences[1];
                _timeLeft.text = preffix + timeDifference.ToString() + suffix;
                _hasAllFishes = false;
                break;
            }
        }

        StartCoroutine(ResultsFishApparition());

        if (_hasAllFishes)
        {
            _timeLeft.text = _AllFishesSentence;
        }

    }

    IEnumerator ResultsFishApparition()
    {
        yield return new WaitForSeconds(_timeBeforeStarsApparition);
        foreach (var fish in _fishTimers)
        {
            FishAppear(fish);
            yield return new WaitForSeconds(_timeBetweenStars);
        }
    }

    private void FishAppear(GameObject fish)
    {
        fish.transform.localScale = Vector2.one * _starStartScale;
        fish.transform.DOScale(Vector2.one, _starApparitionTime).SetEase(Ease.InExpo);
    }
}
