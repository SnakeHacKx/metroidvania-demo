using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private static ChangeBackground _sharedInstance;

    public static ChangeBackground SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<ChangeBackground>();
            }
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    public void ChangeBackgroundTo(Sprite newBackground, Vector3 backgroundScale)
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newBackground;
        this.transform.localScale = backgroundScale;
        //Debug.Log("La posision debe ser 0, 0, 0");
        //this.transform.position = new Vector3(0, 0, 0);
    }
}
