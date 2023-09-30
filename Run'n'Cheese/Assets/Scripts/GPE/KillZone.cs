using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : Enemy
{

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerController>() == null) return;

        Destroy(collider.gameObject);
    }
}
