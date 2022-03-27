using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen _sharedInstance;
    [SerializeField] CanvasFade canvasFade;

    public static LoadingScreen SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<LoadingScreen>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/LoadingScreen") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<LoadingScreen>();
                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    private void Awake()
    {
        // A veces Unity no pone el soarting layer correctamente... Asi que se lo ponemos manualmente
        GetComponent<Canvas>().sortingLayerName = "UI";
    }

    public void OnLoadScreen()
    {
        canvasFade.FadeIn();
    }

    public void OnLoadedScreen()
    {
        canvasFade.FadeOut();
        
    }

    public IEnumerator OnLoadScreenCoroutine()
    {
        yield return canvasFade.FadeInCoroutine();
    }

    public IEnumerator OnLoadedScreenCoroutine()
    {
        Debug.Log("Debio empezar el fade out********************");
        yield return canvasFade.FadeOutCoroutine();
        Destroy(this.gameObject);
    }
}
