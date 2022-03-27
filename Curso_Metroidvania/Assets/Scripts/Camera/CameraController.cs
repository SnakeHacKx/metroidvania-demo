using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class CameraController : MonoBehaviour
{
    //[SerializeField] private float offsetZ;
    [SerializeField] float sizeSpeed = 1; // Velocidad de crecimiento del tamano de la camara
    [SerializeField] Vector2 scaleFactor;
    //[SerializeField] GameObject background;

    
    //private GameObject targetGameObject;

    //private bool freezeCamera = false;

    public CinemachineFreeLook cinemachineFreeLook;

    private static CameraController _sharedInstance;

    public static CameraController SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<CameraController>();
            }
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    public void UpdatePosition(Vector2 position)
    {
        this.transform.position = position;
    }

    public void ChangeCameraSize(float sizeCamera)
    {
        StartCoroutine(ChangeCameraSize_(sizeCamera));
    } 

    IEnumerator ChangeCameraSize_(float sizeCamera)
    {
        var size = Camera.main.orthographicSize; 

        while (size < sizeCamera)
        {
            size += sizeSpeed;

            Camera.main.orthographicSize = size;
            //Debug.Log("Ahora se muestra en pantalla");
            //background.transform.localScale = new Vector3(scaleFactor.x * size, scaleFactor.y * size, 1);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /*public void FreezeCamera()
    {
        freezeCamera = true;
    }*/

/*#if UNITY_EDITOR // Nos evita problemas de compilación, ya que le estamos diciendo al script que
    // sólo compile esta parte en el editor de unity

    // nos permite visualizar mejor lo que estamos haciendo
    private void OnDrawGizmos()
    {
        var pointA = new Vector2(boundaryX.min, boundaryY.min);
        var pointB = new Vector2(boundaryX.min, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryX.max, boundaryY.min);
        pointB = new Vector2(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryX.min, boundaryY.min);
        pointB = new Vector2(boundaryX.max, boundaryY.min);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector2(boundaryX.min, boundaryY.max);
        pointB = new Vector2(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);
    }
#endif*/
}
