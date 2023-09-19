using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private PlayerInputs input;
    [Header("Zoom")]
    [SerializeField] private float zoomTime = 1;
    [SerializeField] private GameObject zoomCamera;
    private bool isZooming;

    private void Awake()
    {
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Zoom.performed += OnZoom;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Zoom.performed -= OnZoom;
    }

    private void OnZoom(InputAction.CallbackContext callback)
    {
        if (!isZooming)
        {
            StartCoroutine(Zooming());
        }
    }

    IEnumerator Zooming()
    {
        isZooming = true;
        zoomCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(zoomTime);
        zoomCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(zoomTime);
        isZooming = false;
    }
}
