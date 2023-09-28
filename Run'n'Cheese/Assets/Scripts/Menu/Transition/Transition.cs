using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Transition : MonoBehaviour
{
    public static Transition Instance;

    public event Action<Action> OnFadeOutStart;

    [Tooltip("The visual representation type of the transition")] [SerializeField] private AnimationMode _transitionMode;
    [Tooltip("Fades in at the beginning")] [SerializeField] private bool _fadeIn;

    [Header("Fade")]
    [Tooltip("The Image used for the Fade transition")] [SerializeField] private Image _fadeImage;
    [Tooltip("The time the fade takes to complete in seconds")] [SerializeField] private float _fadeTime = 1f;

    [Header("Animations")]
    [Tooltip("The Animator used for the Animation transition")] [SerializeField] private Animator _animator;
    [Tooltip("The name of the FadeIn Animation")] [SerializeField] private string _fadeInAnimationName;
    [Tooltip("The name of the FadeOut Animation")] [SerializeField] private string _fadeOutAnimationName;    

    enum AnimationMode
    {
        Fade,
        Animation
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_fadeIn)
        {
            switch (_transitionMode)
            {
                case AnimationMode.Fade:
                    _fadeImage.DOFade(1, 0);
                    _fadeImage.DOFade(0, _fadeTime);
                    break;
                case AnimationMode.Animation:
                    _animator.Play(_fadeInAnimationName);
                    break;
            }
        }
    }

    public void SetTransition(Action endAction)
    {
        StartCoroutine(_startTransition(endAction));
    }

    private IEnumerator _startTransition(Action endAction)
    {
        switch (_transitionMode)
        {
            case AnimationMode.Fade:
                _fadeImage.DOFade(0, 0);
                _fadeImage.DOFade(1, _fadeTime);
                yield return new WaitForSeconds(_fadeTime);
                endAction?.Invoke();
                break;
            case AnimationMode.Animation:
                _animator.Play(_fadeOutAnimationName);
                OnFadeOutStart?.Invoke(endAction);
                yield return null;
                break;
        }
    }
}
