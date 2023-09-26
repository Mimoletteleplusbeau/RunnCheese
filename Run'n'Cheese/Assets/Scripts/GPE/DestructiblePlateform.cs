using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlateform : MonoBehaviour
{
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() == null) return;
        
        var myExplosion = GetComponent<ExplosionObject>();
        if (myExplosion != null) myExplosion.Explode();
        else Destroy(gameObject);
    }
}
