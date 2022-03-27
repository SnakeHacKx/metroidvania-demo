using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVirtualCamera : MonoBehaviour
{
    private static TeleportVirtualCamera _sharedInstance;

    public static TeleportVirtualCamera SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<TeleportVirtualCamera>();
            }
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    public void ChangePosition(Vector3 position)
    {
        var virtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        virtualCamera.ForceCameraPosition(Vector3.Lerp(this.transform.position, position, 1), Quaternion.identity);
        Debug.Log("Debio tepearse la camara a la posicion: " + position);
    }
}
