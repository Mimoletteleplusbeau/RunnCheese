using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationForce;

    private void Update()
    {
        Vector2 mouseVector = new Vector2((Input.mousePosition.y / (Screen.height / 2)) / 2, (Input.mousePosition.x / (Screen.width / 2))) / 2;
        _camera.transform.rotation = Quaternion.Euler(mouseVector * _rotationForce);
    }
}
