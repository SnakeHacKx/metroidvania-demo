using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestItem : MonoBehaviour
{
    public int questID;
    private QuestManager questManager;
    private ItemsManager itemsManager;
    public string itemName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagID.Player.ToString()))
        {
            questManager = FindObjectOfType<QuestManager>();
            itemsManager = FindObjectOfType<ItemsManager>();

            Quest quest = questManager.QuestWithID(questID);

            if(quest == null)
            {
                Debug.LogErrorFormat("La mision con ID {0} no existe", questID);
            }

            if (quest.gameObject.activeInHierarchy && !quest.questCompleted)
            {
                questManager.itemCollected = this;
                itemsManager.AddQuestItem(this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
