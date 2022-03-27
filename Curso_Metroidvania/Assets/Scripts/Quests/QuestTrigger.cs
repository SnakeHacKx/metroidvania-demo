using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    private QuestManager questManager;
    public int questID;
    public bool startPoint, endPoint;

    //[Header("How The Quest Start?")]
    //public bool isDialogueQuest;
    //public bool isZoneQuest;

    [Header("Debug Variables")]
    [SerializeField] private bool playerInZone;
    public bool dialogueFinished = false;
    public bool automaticCatch; // la mision se acepta automaticamente?

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagID.Player.ToString()))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagID.Player.ToString()))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        if (playerInZone)
        {
            if (automaticCatch || (!automaticCatch && (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Joystick1Button3))))
            {
                Quest quest = questManager.QuestWithID(questID); // busca el ID de la mision
                if (quest == null)
                {
                    Debug.LogErrorFormat("La mision con ID {0} no existe", questID);
                    return;
                }
                
                // Si llego aqui, la mision existe
                if (!quest.questCompleted) // Si se elimina esta linea, la mision es repetible
                {
                    Debug.Log("Quest No completada");
                    // Si en el quest manager, en su lista de misiones, en la posicion actual, no ha
                    // complatado la mision
                    if (startPoint)
                    {
                        Debug.Log("Punto de inicio");

                        // Estoy en la zona en la que empieza la mision
                        if (!quest.gameObject.activeInHierarchy)
                        {
                            Debug.Log("Quest activa en la jerarquia");

                            // Si el gameobject que tiene la mision, no esta activo en la jerarquia
                            // esta no se ha activado y puede activarse
                            quest.gameObject.SetActive(true);
                            quest.StartQuest();
                        }
                    }

                    if (endPoint)
                    {
                        Debug.Log("Punto de fin");

                        // Estoy en la zona en la que acaba la mision
                        if (quest.gameObject.activeInHierarchy)
                        {
                            quest.CompleteQuest();
                        }
                    }
                }
            }
        }

     
    }
}
