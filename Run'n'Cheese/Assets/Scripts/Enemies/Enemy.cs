using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PushZone>() != null)
            return;
        Debug.Log(collision.collider.name);
        Destroy(gameObject);
    }
}
