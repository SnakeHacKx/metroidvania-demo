using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El gameObject que contenga este script, no será destruido al cargar una nueva escena.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        // si el player no ha sido creado, no lo destruyas al cambiar de escena
        if (!HeroController.playerCreated)
        {
            DontDestroyOnLoad(this.transform.gameObject);
        }
        else
        {
            // si ya existe una copia del player, el último es destruido
            Destroy(gameObject);
        }
    }
}
