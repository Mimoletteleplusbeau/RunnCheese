using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProBuilder;
using UnityEngine;

public class FadeOutManager : MonoBehaviour
{
    private Action _endAction;

    private void Start()
    {
        Transition.Instance.OnFadeOutStart += SaveEndAction;
    }

    public void InvokeAction()
    {
        _endAction?.Invoke();
    }

    private void SaveEndAction(Action endAction)
    {
        _endAction = endAction;
    }
}
