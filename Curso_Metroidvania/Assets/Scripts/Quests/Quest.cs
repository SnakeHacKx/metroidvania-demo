using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Quest : MonoBehaviour
{
    public int questID;
    public bool questCompleted;
    private QuestManager questManager;

    [Header("General Quest Settings")]
    public bool showTextBoxes;
    // todo: poner un booleano para saber si se requiere mostrar el texto de mision completadas
    [Tooltip("Esta mision abre algun camino?")]
    public bool openAWay;
    //[Tooltip("Bloque que abre un camino despues de haber completado la mision")]
    public GameObject block;
    public string questTitle;
    public string questStartText;
    public string questCompleteText;

    [Header("Enemy Killing Quest")]
    public bool killsEnemy; // La mision requiere matar enemigos?
    public List<QuestEnemy> enemies;
    public List<int> numberOfEnemies; // cuantos enemigos de cada tipo, se requieren que sean matados
                                      // Esto se podria hacer con los items, si es que requiero varios items para
                                      // completar la mision

    [Header("Find Items Quest")]
    public bool needsItem; // La mision necesita un item?
    public List<QuestItem> itemsNeeded;

    public Quest nextQuest; // Siguiente mision en la jerarquia (si es que la hay)

    void Start()
    {
        // no se puede poner aqui, ya que por defecto la quest esta desactivada
        // questManager = FindObjectOfType<QuestManager>();
    }

    private void Update()
    {
        //Debug.Log("El bloqueo esta activo en la jerarquia?: " + block.activeInHierarchy);
        if (needsItem && questManager.itemCollected != null)
        {
            // Puedo mirar si en la lista de items esta el que se necesita
            for(int i = 0; i < itemsNeeded.Count; i++)
            {
                if (itemsNeeded[i].itemName == questManager.itemCollected.itemName)
                {
                    itemsNeeded.RemoveAt(i); // Lo elimino porque ya lo que encontrado
                    questManager.itemCollected = null; // El manager anula el item que necesitaba porque ya lo tengo
                    break;   
                }
            }

            if (itemsNeeded.Count == 0)
            {
                //if (openAWay)
                //{
                //    for (int i = 0; i < blocks.Count; i++)
                //    {
                //        Debug.Log(blocks[0].blockName + " , " + lastItemNameCollected);
                //        if (blocks[i].blockName == lastItemNameCollected)
                //        {
                //            Debug.Log("Se ha desactivado el bloqueo con nombre: " + blocks[i].blockName);
                //            blocks[i].gameObject.SetActive(false);
                //            blocks.RemoveAt(i); // Lo elimino porque ya lo que encontrado
                            
                //            break;
                //        }
                //    }
                //    Debug.Log("Se ha desactivado el bloqueo");
                //}

                CompleteQuest();
                Debug.Log("Se ha completado la mision");
            }

            if (killsEnemy && questManager.lastEnemyKilled != null)
            {
                Debug.Log("Tenemos un enemigo recien matado");
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].enemyName == questManager.lastEnemyKilled.enemyName)
                    {
                        numberOfEnemies[i]--;
                        questManager.lastEnemyKilled = null;

                        if (numberOfEnemies[i] <= 0)
                        {
                            enemies.RemoveAt(i);
                            numberOfEnemies.RemoveAt(i);
                        }
                        break;
                    }
                }

                if (enemies.Count == 0)
                {
                    // No me quedan mas enemigos que matar, he completado la mision
                    CompleteQuest();
                }
            }
        }
    }

    /// <summary>
    /// Cuando la escena ha sido cargada, se invoca el OnSceneWasLoaded
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }

    /// <summary>
    /// Cuando la nueva escena se cargue, si hay misiones activas, se cargaran los items o los enemigos.
    /// </summary>
    /// <remarks>
    /// Si no se hace asi, solo funcionaria para la escena actual...
    /// </remarks>
    /// <param name="level">Nuevo nivel o escena.</param>
    /// <param name="mode">Modo sincrono o asincrono.</param>
    private void OnSceneWasLoaded(Scene level, LoadSceneMode mode)
    {
        if (needsItem)
        {
            ActivateItems();
        }

        if (killsEnemy)
        {
            ActivateEnemies();
        }
    }

    public void StartQuest()
    {
        
        questManager = FindObjectOfType<QuestManager>();

        if (showTextBoxes)
            questManager.ShowQuestText(questTitle + "\n" + questStartText);

        if (needsItem) // Busca los items de una mision especificada por el questID y los activa cuando empieza la mision
        {
            ActivateItems();
        }

        if (killsEnemy) // Busca los enemigos de una mision especificada por el questID y los activa cuando empieza la mision
        {
            ActivateEnemies();
        }
    }

    void DesactivateBlocks()
    {
        Object[] questBlocks = Resources.FindObjectsOfTypeAll(typeof(QuestBlocks));
        foreach (QuestBlocks block in questBlocks)
        {
            if (block.questID == questID)
            {
                block.gameObject.SetActive(false);
            }
        }
    }

    void ActivateItems()
    {
        Object[] questItems = Resources.FindObjectsOfTypeAll(typeof(QuestItem));
        foreach (QuestItem item in questItems)
        {
            if (item.questID == questID)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    void ActivateEnemies()
    {
        Object[] questEnemies = Resources.FindObjectsOfTypeAll(typeof(QuestEnemy));
        foreach (QuestEnemy enemy in questEnemies)
        {
            if (enemy.questID == questID)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }

    public void CompleteQuest()
    {
        questManager = FindObjectOfType<QuestManager>();
        questManager.ShowQuestText(questTitle + "\n" + questCompleteText);
        questCompleted = true;

        if (nextQuest != null)
        {
            Invoke(nameof(ActivateNextQuest), 5.0f); // Llama al metodo ActivateNextQuest() 5s despues
        }

        if (openAWay)
        {
            Debug.Log("Se desactivo el bloqueo al terminar la mision");
            DesactivateBlocks();
        }

        gameObject.SetActive(false); // la mision se desactiva, si no se hace, la mision seria repetible
    }

    private void ActivateNextQuest()
    {
        nextQuest.gameObject.SetActive(true);
        nextQuest.StartQuest();
    }
}
