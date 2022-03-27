using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private HeroController player;
    //private CameraFollow theCamera;
    //public Vector2 facingDirection = Vector2.zero;//el jugador mirará a la dirección correcta despues de entrar
    //o salir de un lugar
    [Tooltip("Punto de inicio de la escena, debe ser igual al uuid del TP de la escena de la cual se teletranporta")]
    [SerializeField] private StartAndTeleportPointsID uuid;

    // Start is called before the first frame update
    void Start()
    {
        //Los unicos gameObjects que tienen estos sripts son el player y la cámara... si hubiese
        //más gameObjects con estos scripts, esto no funcionaría
        //esto es pa referenciarlos, o sea pa llamarlos pue
        //player = FindObjectOfType<HeroController>();
        //theCamera = FindObjectOfType<CameraFollow>();

        //Debug.Log("EMPEZÓ");
        //if(HeroController.SharedInstance.nextUuid == null)
        //{
        //    Debug.Log("NEXT UUID ES NULO");
        //}
        //Debug.Log("uuid del TP de la escena anterior: " + HeroController.SharedInstance.nextUuid);
        //Debug.Log("uuid del StartPoint de la escena actual: " + uuid.ToString());

        //Debug.Log("Posición del StartPoint: " + this.transform.position);

        /*if (HeroController.SharedInstance.nextUuid == "")
        {
            Debug.Log("EL UUID ES NULO");
        }*/
           
;
        if (GameManager.SharedInstance.nextUuid != uuid)
        {
            //Debug.Log("UUID INCORRECTO:");
            //Debug.LogFormat("EL uuid actual es: {0} y se compara con: {1}", GameManager.SharedInstance.nextUuid, uuid.ToString());
            //Debug.Log("No son iguales");
            return; //si el jugador tiene por siguiente uuid un nombre distinto al actual, no se hace nada
            //no ejecuta lo que resta de código
        }
        //Debug.Log("UUID CORRECTO:");
        //Debug.LogFormat("EL uuid actual es: {0} y se compara con: {1}", GameManager.SharedInstance.nextUuid, uuid.ToString());
        //Debug.Log("El punto correcto es el de arriba");

        Debug.Log("La posición en la que debería aparecer el jugador al cambiar de escena es: " + this.transform.position);
        HeroController.SharedInstance.transform.position = this.transform.position;
        //Debug.Log("La posición que tomó el Player es: " + HeroController.SharedInstance.transform.position);
        //TeleportVirtualCamera.SharedInstance.GetComponent<TeleportVirtualCamera>();
        TeleportVirtualCamera.SharedInstance.ChangePosition(this.transform.position);
        CameraController.SharedInstance.transform.position = new Vector3(this.transform.position.x,
                                                                         this.transform.position.y,
                                                                         -10);

        //player.lastMovement = facingDirection;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 0.3f);
    }
#endif
}

