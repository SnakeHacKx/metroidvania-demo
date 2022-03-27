using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNewPlace : MonoBehaviour
{
    [Tooltip("Escena a la que el jugador será teletransportado")]
    [SerializeField] private SceneID levelToLoad;
    public bool needsClick = false;
    [Tooltip("Referencia del teletransportador de la escena desde la cual se quiere teletrasnportar, " +
             "debe ser igual al StartPoint de la escena a la cual se quiere teletransportar")]
    [SerializeField] private StartAndTeleportPointsID uuid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("El UUID del TP de la escena level1_4 es: " + uuid.ToString());
        Teleport(collision.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Teleport(collision.gameObject.name);
    }

    private void Teleport(string objName) //objName = nombre del objeto
    {
        if (objName == "Hero")
        {
            if (!needsClick || needsClick && Input.GetMouseButtonDown(0))
            {
                //HeroController.SharedInstance.nextUuid = null;
                //Debug.Log("El UUID del TP de la escena level1_4 es: " + uuid.ToString());
                GameManager.SharedInstance.nextUuid = uuid;
                //Debug.Log("El UUID asignado al HeroController es: " + GameManager.SharedInstance.nextUuid);
                SceneManager.LoadScene(levelToLoad.ToString());
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 10; i++)
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(i * 0.3f, i * 0.3f, 0));
        }
        Handles.Label((Vector2)this.transform.position + Vector2.up * 2, "LEVEL: " + levelToLoad.ToString());
    }
#endif
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el cambio de escenas y los puntos de inicio y teletransporte.
/// <list type="bullet">
/// <item>
/// <term>Teleport</term>
/// <description>Teletransporta al jugador al punto especificado.</description>
/// </item>
/// </list>
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class GoToNewPlace : MonoBehaviour
{
    [Tooltip("Escena a la que el jugador será teletransportado")]
    [SerializeField] private SceneID levelToLoad;

    [Tooltip("¿Necesita el jugador una tecla especificada para irse a una nueva escena?")]
    [SerializeField] private bool needsClick;

    //private SetCameraConfiner _setCameraConfiner;

    // identificador del start point al cual se quiere ir
    // public string uuid;

    [Tooltip("Referencia del teletransportador de la escena desde la cual se quiere teletrasnportar, " +
             "debe ser igual al StartPoint de la escena a la cual se quiere teletransportar")]
    [SerializeField] private StartAndTeleportPointsID uuid;

    //private void Start()
    //{
    //    _setCameraConfiner = GetComponent<SetCameraConfiner>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Teleport(collision.gameObject.tag);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Teleport(collision.gameObject.tag);
    }

    /// <summary>
    /// Teletransporta al jugador a una nueva escena.
    /// </summary>
    /// <param name="objectTag">Contiene el tag de la colisión.</param>
    private void Teleport(string objectTag)
    {
        // Si el tag del gameObject que entra en collision con el collider que tenga
        // este script es "Player", entonces cargo una nueva escena
        if (objectTag == TagID.Player.ToString())
        {
            if (!needsClick || (needsClick && Input.GetKeyDown(KeyCode.E)))
            {
                // indica que el siguiente punto de teletransporte, es el que se le ha
                // puesto al GoToNewPlace
                FindObjectOfType<HeroController>().nextUuid = uuid.ToString();

                //SceneHelper.SharedInstance.LoadScene(levelToLoad);
                SceneManager.LoadScene(levelToLoad.ToString());
            }
        }
    }
}*/
