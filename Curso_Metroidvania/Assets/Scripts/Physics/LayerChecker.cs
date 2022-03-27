using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    public enum LayerCheckerType
    {
        Ray,
        Circle
    }

    [SerializeField] private LayerCheckerType layerCheckerType;
    //[SerializeField] private TagID tagCheckerType;
    [SerializeField] private LayerMask targetMask; // Layer que queremos checar
    [SerializeField] private Vector2 direction; // dirección del rayo

    [Tooltip("Distancia del rayo o Radio del círculo")]
    [SerializeField] private float distance;

    public bool isTouching;
    
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (layerCheckerType == LayerCheckerType.Ray)
            isTouching = Physics2D.Raycast(this.transform.position, direction, distance, targetMask);

        if (layerCheckerType == LayerCheckerType.Circle)
            isTouching = Physics2D.OverlapCircle(this.transform.position, distance, targetMask);
    }

#if UNITY_EDITOR // Nos evita problemas de compilación, ya que le estamos diciendo al script que
    // sólo compile esta parte en el editor de unity

    // nos permite visualizar mejor lo que estamos haciendo
    private void OnDrawGizmos()
    {
        if (isTouching)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        if (layerCheckerType == LayerCheckerType.Ray)
            Gizmos.DrawRay(this.transform.position, direction * distance);

        if (layerCheckerType == LayerCheckerType.Circle)
            Gizmos.DrawWireSphere(this.transform.position, distance);
    }
#endif
}
