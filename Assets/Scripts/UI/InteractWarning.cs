using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractWarning : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] CanvasFade canvasFade;
    [SerializeField] float fadeSpeed;

    private static InteractWarning _sharedInstance;

    public static InteractWarning SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<InteractWarning>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/InteractWarning") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<InteractWarning>();
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

    public void ShowInteractWarning()
    {
        canvasFade.FadeIn();
    }

    public void HideInteractWarning()
    {
        canvasFade.FadeOut();
        Destroy(this.gameObject);
    }
}
