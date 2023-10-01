using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [field: SerializeField] protected abstract GameObject _VFXDeath { get; set; }

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

    protected virtual void SpawnDeathVFX()
    {
        GameObject vfx = Instantiate(_VFXDeath, transform.position, Quaternion.identity);
        vfx.transform.SetParent(transform.parent);
    }
}
