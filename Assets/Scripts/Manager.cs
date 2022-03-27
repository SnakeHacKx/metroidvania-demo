using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject sceneManager;
    [SerializeField] GameObject audioManager;
    [SerializeField] GameObject dialogueManager;
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject levelManager;
    [SerializeField] GameObject questManager;
    [SerializeField] GameObject hero;

    private void Start()
    {
        GameManager.SharedInstance.gameObject.SetActive(true);
        AudioManager.SharedInstance.gameObject.SetActive(true);
        //HeroController.SharedInstance.gameObject.SetActive(true);
        //UIManager.SharedInstance.gameObject.SetActive(true);
    }
}
