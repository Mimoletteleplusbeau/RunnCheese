using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExplosionObject : MonoBehaviour
{

    public GameObject originalObject;
    public GameObject fracturedObject;

    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRadius = 10;
    public float fragScaleFactor = 1;

    public void Explode()
    {
        SoundsList.Instance.PlayPlateformBreak();

        Destroy(gameObject.GetComponent<BoxCollider2D>());
        if (originalObject != null)
        {
            originalObject.SetActive(false);

            if (fracturedObject != null)
            {
                fracturedObject.SetActive(true);
                foreach (Transform t in fracturedObject.transform)
                {
                    var rb = t.GetComponent<Rigidbody>(); 

                    if (rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);

                    //StartCoroutine(Shrink(t, 2));
                }

                Destroy(fracturedObject, 5);
            }
        }
        Destroy(gameObject, 10);
    }

    IEnumerator Shrink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 newScale = t.localScale;

        while (newScale.x >= 0)
        {
            newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);
            if (t!=null)
                t.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
