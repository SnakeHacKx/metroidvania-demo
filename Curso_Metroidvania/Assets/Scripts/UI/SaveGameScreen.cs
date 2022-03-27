using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISaveGameScreen
{
    void OnHideScreen();
}

public class SaveGameScreen : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] CanvasFade canvasFade;
    [SerializeField] float fadeSpeed;
    [SerializeField] AudioClip buttonSFX;

    private static SaveGameScreen _sharedInstance;

    [Header("Control Menu")]
    [SerializeField] GameObject mainMenuFirstButton;

    public static SaveGameScreen SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<SaveGameScreen>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/SaveGameScreen") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<SaveGameScreen>();
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

    private ISaveGameScreen saveGameScreen;
    public void ShowSaveGameScreen(ISaveGameScreen saveGameScreen)
    {
        this.saveGameScreen = saveGameScreen;
        canvasFade.FadeIn();
        // limpiar objeto seleccionado (hay que hacer esto si no, da error.. cosas raras de Unity)
        EventSystem.current.SetSelectedGameObject(null);

        // Poner un nuevo objeto seleccionado
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }

    // ESTO ARREGLARLO PARA NO REPETIR CODIGO (TENGO LO MISMO EN LOADING SCREEN)
    public void FadeIn()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        canvasGroup.alpha = 0;
        var alpha = canvasGroup.alpha;

        while (alpha < 1)
        {
            alpha += fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        canvasGroup.alpha = 1;
    }

    public void FadeOut()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
        canvasGroup.alpha = 1;
        var alpha = canvasGroup.alpha;

        while (alpha > 0)
        {
            alpha -= fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canvasGroup.alpha = 0;
    }

    public void SaveGameButton()
    {
        GameManager.SharedInstance.SaveGame();
        StartCoroutine(SaveGameButtonCoroutine());
    }

    public IEnumerator SaveGameButtonCoroutine()
    {
        yield return canvasFade.FadeOutCoroutine();
        // Cuando el jugador ya haya dado click, podemos esconder la pantalla
        this.saveGameScreen.OnHideScreen();
        Destroy(this.gameObject);
    }

    public void NoSaveGameButton()
    {
        StartCoroutine(NoSaveGameButtonCoroutine());
    }

    public IEnumerator NoSaveGameButtonCoroutine()
    {
        yield return canvasFade.FadeOutCoroutine();
        this.saveGameScreen.OnHideScreen();
        Destroy(this.gameObject);
    }
}
