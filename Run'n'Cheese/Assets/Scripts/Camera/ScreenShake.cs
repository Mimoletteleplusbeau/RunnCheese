using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;
    [Tooltip("The strength of the screenshake")][SerializeField] private float strength = 1f;
    [Tooltip("The length of the screenshake in seconds")][SerializeField] private float duration = 1f;
    [Tooltip("The curve of the screenshake's strength over time")][SerializeField] private AnimationCurve curve;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(float _strength = -1, float _duration = -1, AnimationCurve _curve = null)
    {
        StartCoroutine(Shaking(_strength, _duration, _curve));
    }

    private IEnumerator Shaking(float _strength = -1, float _duration = -1, AnimationCurve _curve = null)
    {
        if (_strength == -1) _strength = strength;
        if (_duration == -1) _duration = duration;
        if (_curve == null) _curve = curve;
        //Debug.Log($"Shake force: {_strength}, Length: {_duration}, Curve: {_curve}");

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float _curveStrength = _curve.Evaluate(elapsedTime / _duration);
            transform.position = startPosition + Random.insideUnitSphere * _curveStrength * _strength;
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z);
            yield return null;
        }
        transform.position = startPosition;
    }
}
