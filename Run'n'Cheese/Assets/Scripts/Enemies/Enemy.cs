using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PushZone>() == null)
            return;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == null)
            return;
        Debug.Log(collision.gameObject.name);
    }
}
