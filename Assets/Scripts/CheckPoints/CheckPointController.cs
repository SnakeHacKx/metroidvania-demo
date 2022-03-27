using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour, ISaveGameScreen
{
    private bool screenIsShowing;

    private bool playerInTheZone;

    public void OnHideScreen()
    {
        screenIsShowing = false;
        HeroController.SharedInstance.SetIsControlable(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagID.Player.ToString()) && !screenIsShowing)
        {
            playerInTheZone = true;
            InteractWarning.SharedInstance.ShowInteractWarning();
            /*SaveGameScreen.SharedInstance.ShowSaveGameScreen(this);
            HeroController.SharedInstance.SetIsControlable(false);*/
            
            //screenIsShowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractWarning.SharedInstance.HideInteractWarning();
        playerInTheZone = false;
    }

    private void Update()
    {
        if (playerInTheZone && (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Joystick1Button3)))
        {
            GameManager.SharedInstance.lastCheckpointPos = transform.position;
            GameManager.SharedInstance.SaveGame();
            PopUpMessage.SharedInstance.ShowPopUpMessage();
            //Debug.Log("Partida Guardada en la posición " + transform.position);
        }
    }


    // Este ya no tiene sentido gracias a la interface

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag(TagID.Player.ToString()) && screenIsShowing)
    //    {
    //        SaveGameScreen.SharedInstance.ShowGameOverScreen();
    //        HeroController.SharedInstance.SetIsControlable(true);
    //        screenIsShowing = false;
    //    }
    //}
}
