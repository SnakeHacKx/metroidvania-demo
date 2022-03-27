using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 3f; //tiempo de vida del gameObject
    [SerializeField] AudioClip Sfx;

    private void Awake()
    {
        if(Sfx != null)
        {
            AudioManager.SharedInstance.PlaySFX(Sfx);
        }

        Destroy(this.gameObject, timeToDestroy);
    }
}
