using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSurroundings : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    private static SpikeSurroundings _sharedInstance;

    public static SpikeSurroundings SharedInstance
    {
        get
        {
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawnPoint != null)
            HeroController.SharedInstance.LastPositionOnGround = spawnPoint.transform.position;
        else
            Debug.LogWarning("El spawnPoint es nulo");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
            Gizmos.DrawWireSphere(spawnPoint.transform.position, 0.2f);
    }
#endif
}
