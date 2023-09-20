using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyParticles : MonoBehaviour
{
    private float _destroyTime = 10f;
    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
}