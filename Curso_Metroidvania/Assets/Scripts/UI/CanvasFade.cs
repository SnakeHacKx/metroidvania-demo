using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float fadeSpeed;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
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

        //Debug.Log("Termino el fade in");

        /*if(PauseScreen.SharedInstance != null)
            if(PauseScreen.SharedInstance.gamePaused == true)
            GameManager.SharedInstance.GamePaused = true;*/
    }

    public void FadeOut()
    {
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
}
