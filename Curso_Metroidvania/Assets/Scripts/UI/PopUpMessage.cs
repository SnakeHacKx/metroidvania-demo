using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpMessage : MonoBehaviour
{
    //CanvasGroup canvasGroup;
    [SerializeField] CanvasFade canvasFade;
    [SerializeField] float fadeSpeed;
    [SerializeField] float timeToDestroy = 3f;

    float counter = 0;

    private static PopUpMessage _sharedInstance;

    public static PopUpMessage SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<PopUpMessage>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/PopUpMessage") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<PopUpMessage>();
                    var eventSystem = FindObjectOfType<EventSystem>();

                    if (eventSystem == null)
                    {
                        var gameObjES = new GameObject("EventSystem");
                        gameObjES.AddComponent<EventSystem>();
                        gameObjES.AddComponent<StandaloneInputModule>();
                    }
                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    private void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;
        //Debug.Log(Time.fixedDeltaTime);
        if (counter > timeToDestroy)
            HidePopUpMessage();
    }

    public void ShowPopUpMessage()
    {
        canvasFade.FadeIn();
        //HidePopUpMessage();
    }

    public void HidePopUpMessage()
    {
        //canvasFade.FadeOut();
        //Debug.Log("Se destruyó");
        Destroy(this.gameObject, timeToDestroy);
    }
}
