using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] float sizeSpeed = 1; // Velocidad de crecimiento del tamano de la camara
    [SerializeField] Vector2 scaleFactor;

    private static CameraManager _sharedInstance;
   
    public static CameraManager SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<CameraManager>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = new GameObject("CameraManager");
                    gameObj.AddComponent<CameraManager>();
                    _sharedInstance = gameObj.GetComponent<CameraManager>();
                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    public void SetActiveBackground(bool active)
    {
        /*if (active)
            background.SetActive(true);
        else if (!active)
            background.SetActive(false);*/
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
            background.transform.localScale = new Vector3(scaleFactor.x * size, scaleFactor.y * size, 1);
            //GetSize();

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
