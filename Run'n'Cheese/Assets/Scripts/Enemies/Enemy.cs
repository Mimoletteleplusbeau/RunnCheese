using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PushZone>() != null)
        {
            Destroy(gameObject);
            collider.gameObject.GetComponent<PushZone>().AddBullets(1);
        } else if (collider.gameObject.GetComponent<PlayerController>() != null)
        {
            Destroy(collider.gameObject);
        }
    }
}
