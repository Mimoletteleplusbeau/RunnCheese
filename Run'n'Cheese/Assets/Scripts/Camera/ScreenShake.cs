using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;
    private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    [Tooltip("The strength of the screenshake")][SerializeField] private float strength = 1f;
    [Tooltip("The length of the screenshake in seconds")][SerializeField] private float duration = 1f;
    [Tooltip("The curve of the screenshake's strength over time")][SerializeField] private AnimationCurve curve;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
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

        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _strength;
        yield return new WaitForSeconds(_duration);
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        //Vector3 startPosition = transform.localPosition;
        //float elapsedTime = 0f;

        //while (elapsedTime < _duration)
        //{
        //    elapsedTime += Time.deltaTime;
        //    float _curveStrength = _curve.Evaluate(elapsedTime / _duration);
        //    transform.localPosition = startPosition + Random.insideUnitSphere * _curveStrength * _strength;
        //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startPosition.z);
        //    yield return null;
        //}

        //Debug.Log(transform.localPosition);
        //transform.localPosition = startPosition;
    }
}
