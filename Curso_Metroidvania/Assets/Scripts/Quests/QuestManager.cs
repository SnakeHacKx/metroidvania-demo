using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests; // se podria hacer privada
    private DialogueManager dialogueManager;

    public QuestItem itemCollected;
    public QuestEnemy lastEnemyKilled; // ultimo enemigo que hemos matado

    [SerializeField] GameObject block;
    GameObject instance;
    Vector3 postion = new Vector3(268, 0.4f, 0);

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        foreach(Transform t in transform)
        {
            // Obtengo los hijos del GameManager, o sea, las quests
            quests.Add(t.gameObject.GetComponent<Quest>());
        }        
    }

    public void ShowQuestText(string questText)
    {
        if (dialogueManager == null)
        {
            Debug.LogError("El dialogue manager es nulo");
        }
        // como el ShowDialogue acepta toda una coleccion (un array), le pasamos un array de un solo
        // elemento para que no de error
        dialogueManager.ShowDialogue(new string[] { questText });
    }

    /// <summary>
    /// Metodo que ayuda a elegir la quest, de entre la lista completa de quests,
    /// a la que se esta haciendo referencia, ya que cuando se tienen muchas
    /// quests, se tiende uno a confundir.
    /// </summary>
    /// <param name="questID">questID de la quest que se quiere hacer referencia.</param>
    /// <returns>quest ID que se esta buscando, si no se encuentra, devuelve null.</returns>
    public Quest QuestWithID(int questID)
    {
        Quest q = null;
        //q = GetComponent<Quest>();

        if(quests == null)
        {
            Debug.Log("La lista es nula");
        }

        foreach(Quest temp in quests)
        {
            if(temp.questID == questID)
            {
                q = temp;
            }
        }

        return q;
    }
}
