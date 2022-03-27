using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AlphaBlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 10;
    private Tilemap _tilemapRenderer;

    private SpriteRenderer _spriteRenderer;

    private bool playBlinkEffect;

    Color color;

    private void Awake()
    {
        if(GetComponent<Tilemap>() != null)
        {
            _tilemapRenderer = GetComponent<Tilemap>();
            color = _tilemapRenderer.color;
        }

        if (GetComponent<SpriteRenderer>() != null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            color = _spriteRenderer.color;
        }
    }

    public void PlayDamegeEffect()
    {
        StartCoroutine(_PlayDamageEffect());
    }

    public IEnumerator _PlayDamageEffect()
    {
        color.a = 0;

        if(_tilemapRenderer != null)
        {
            _tilemapRenderer.color = color;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }

        yield return new WaitForSeconds(0.3f);

        color.a = 1;

        if (_tilemapRenderer != null)
        {
            _tilemapRenderer.color = color;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }
    }

    public void PlayBlinkEffect()
    {
        playBlinkEffect = true;
        StartCoroutine(_PlayBlinkEffect());
    }

    public void StopBlinkEffect()
    {
        playBlinkEffect = false;

        color.a = 1;

        ChangeColor();
    }

    private void ChangeColor()
    {
        if (_tilemapRenderer != null)
        {
            _tilemapRenderer.color = color;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }
    }

    public void BecomeInvisible()
    {
        playBlinkEffect = false;

        color.a = 0;

        ChangeColor();
    }

    public void BecomeVisible()
    {
        playBlinkEffect = false;

        color.a = 1;

        ChangeColor();
    }

    private IEnumerator _PlayBlinkEffect()
    {
        float cosenoValue;

        while (playBlinkEffect)
        {
            // Esto va desde -1 a 1, pero no nos interesa el valor de -1
            cosenoValue = Mathf.Cos(Time.time * blinkSpeed);

            color.a = cosenoValue < 0 ? 0 : cosenoValue;

            if (_tilemapRenderer != null)
            {
                _tilemapRenderer.color = color;
            }

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = color;
            }

            yield return !playBlinkEffect;
        }
    }
}
