using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Shadow : MonoBehaviour
{
    private DecalProjector _projector;
    [SerializeField] private float _maxDistance;

    private void Awake()
    {
        _projector = GetComponent<DecalProjector>();
    }
    private void Start()
    {
        //Debug.Log(_projector.drawDistance);
        //Debug.Log(_projector.size.x);
        //Debug.Log(_projector.size.y);
        Debug.Log(_projector.uvScale); // 1 1
        Debug.Log(_projector.uvBias); // 0 0
    }

    [SerializeField] private Vector2 uvBias;
    [SerializeField] private Vector2 uvScale;
    [SerializeField] private LayerMask _plateformLayer;

    void Update()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(_projector.transform.position, Vector2.down, _maxDistance, _plateformLayer);
        if (raycastHit2D.collider == null) return;

        float distance = _projector.transform.position.y - raycastHit2D.transform.position.y;
        //Debug.Log(distance);
        Debug.DrawLine(new Vector3(_projector.transform.position.x, _projector.transform.position.y), new Vector3(_projector.transform.position.x, raycastHit2D.transform.position.y), Color.red);
    }
}
