using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{

    //public GameObject originalObject;
    //public GameObject fracturedObject;

    //public float explosionMinForce = 5;
    //public float explosionMaxForce = 100;
    //public float explosionForceRadius = 10;
    //public float fragScaleFactor = 1;

    //private GameObject fractObj;

    //void Update() // Input de key pour les tests à remplacer par un test de colison avec le projectile
    //{
    //    if (Input.GetKeyDown(keyCode.Space)) // Pas compris l'erreur... --> KeyCode existe pas ? 
    //    {
    //        Explode();
    //    }
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        Reset();
    //    }
    //}

    //void Explode()
    //{
    //    if (originalObject != null)         // Pas utile si tu fais deja ça dans le script original
    //    {
    //        originalObject.SetActive(false); // La même ^

    //        if (fracturedObject != null)
    //        {
    //            fractObj = Instantiate(fracturedObject) as GameObject;

    //            foreach (Transform t in fractObj.transform)
    //            {
    //                var rb = t.GetComponent<Rigbody>(); // Rigbody n'existe pas et je sais pas comment le corriger 
                    
    //                if (rb != null)
    //                    rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);

    //                StartCoroutine(Shrink(t, 2));
    //            }

    //            Destroy(fractObj, 5);


    //        }
    //    }
    //}

    //void Reset() // A supr pour le code après 
    //{
    //    Destroy(fractObj);
    //    originalObject.SetActive(true);
    //}

    //IEnumerator Shrink (Transform t, float delay) // Vanish des props 
    //{
    //    yield return new WaitForSeconds(delay);

    //    Vector3 newScale = t.localScale;

    //    while (newScale.x >= 0)
    //    {
    //        newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);

    //        t.localScale = newScale;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}
 
}
