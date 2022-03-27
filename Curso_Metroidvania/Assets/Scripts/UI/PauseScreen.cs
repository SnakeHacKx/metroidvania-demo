using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{

    CanvasGroup canvasGroup;
    [SerializeField] CanvasFade canvasFade;
    [SerializeField] float fadeSpeed;
    [SerializeField] AudioClip buttonSFX;

    private static PauseScreen _sharedInstance;
    public bool gamePaused = false;

    [Header("Control Menu")]
    [SerializeField] GameObject pauseFirstButton;

    public static PauseScreen SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<PauseScreen>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/PauseScreen") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<PauseScreen>();
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

    public void PauseGame()
    {
        
        GameManager.SharedInstance.GamePaused = true;
        gamePaused = true;
    }

    public void ShowPauseScreen()
    {
        canvasFade.FadeIn();
        Invoke(nameof(PauseGame), 0.1f);
        // limpiar objeto seleccionado (hay que hacer esto si no, da error.. cosas raras de Unity)
        EventSystem.current.SetSelectedGameObject(null);

        // Poner un nuevo objeto seleccionado
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        Debug.Log("Se puso pausa");
        //GameManager.SharedInstance.GamePaused = true;
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

    public void Continue()
    {
        if (buttonSFX != null)
            AudioManager.SharedInstance.PlaySFX(buttonSFX);

        //SceneHelper.SharedInstance.ReloadScene();
        GameManager.SharedInstance.HidePauseScreen();
        HeroController.SharedInstance.SetIsControlable(true);
        //HeroController.SharedInstance.SetIsControlable(true);
        Destroy(this.gameObject);
    }

    public void ExitToMainMenu()
    {
        if (buttonSFX != null)
            AudioManager.SharedInstance.PlaySFX(buttonSFX);

        GameManager.SharedInstance.HidePauseScreen();
        SceneHelper.SharedInstance.LoadScene(SceneID.TitleScreen);
        Destroy(this.gameObject);
    }
}
