using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlateform : Enemy
{
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Blobfish>() == null) return;
        Destroy(gameObject);
    }
}
