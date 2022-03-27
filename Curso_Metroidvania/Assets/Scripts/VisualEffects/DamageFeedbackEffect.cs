using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackEffect : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 10;
    private Renderer _renderer;

    private bool playBlinkEffect;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void PlayDamageEffect()
    {
        StartCoroutine(_PlayDamageEffect());
    }

    public IEnumerator _PlayDamageEffect()
    {
        _renderer.material.SetFloat("_FlashAmount", 1); // torna blanco el sprite

        yield return new WaitForSeconds(0.3f);

        _renderer.material.SetFloat("_FlashAmount", 0); // lo regresa a la normalidad
    }

    public void PlayBlinkDamageEffect()
    {
        if (!playBlinkEffect)
        {
            playBlinkEffect = true;
            StartCoroutine(_PlayBlinkDamageEffect());
        }     
    }
    
    // Para el Final Boss
    public void PlayBlinkDamageEffect(float time)
    {
        playBlinkEffect = true;
        StartCoroutine(_PlayBlinkDamageEffect(time));
    }

    public void StopBlinkDamageEffect()
    {
        playBlinkEffect = false;

        _renderer.material.SetFloat("_FlashAmount", 0);
    }

    private IEnumerator _PlayBlinkDamageEffect(float time)
    {
        float cosenoValue;
        float timeTemp = 0;

        while (playBlinkEffect)
        {
            // Esto va desde -1 a 1, pero no nos interesa el valor de -1
            cosenoValue = Mathf.Cos(Time.time * blinkSpeed);

            _renderer.material.SetFloat("_FlashAmount", cosenoValue < 0 ? 0 : cosenoValue);

            timeTemp += Time.deltaTime;

            if(timeTemp > time)
            {
                playBlinkEffect = false;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopBlinkDamageEffect();
    }

    private IEnumerator _PlayBlinkDamageEffect()
    {
        float cosenoValue;

        while (playBlinkEffect)
        {
            // Esto va desde -1 a 1, pero no nos interesa el valor de -1
            cosenoValue = Mathf.Cos(Time.time * blinkSpeed);

            _renderer.material.SetFloat("_FlashAmount", cosenoValue < 0 ? 0 : cosenoValue);

            yield return !playBlinkEffect;
        }
    }
}
