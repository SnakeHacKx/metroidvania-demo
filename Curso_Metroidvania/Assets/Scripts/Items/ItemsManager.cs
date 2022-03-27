using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private List<GameObject> questItems = new List<GameObject>();
    public List<GameObject> GetQuestItems() { return questItems; }

    public QuestItem GetItemAt(int index) { return questItems[index].GetComponent<QuestItem>(); }

    public bool HasTheQuestItem(QuestItem item)
    {
        foreach (GameObject questItem in questItems)
        {
            if (questItem.GetComponent<QuestItem>().itemName == item.itemName)
            {
                return true;
            }
        }

        return false;
    }

    public void AddQuestItem(GameObject newItem)
    {
        questItems.Add(newItem);
    }
}
