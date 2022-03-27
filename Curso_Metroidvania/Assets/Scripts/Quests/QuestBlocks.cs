using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBlocks : MonoBehaviour
{
    public int questID;
    private QuestManager questManager;
    public string blockName;

    private static QuestBlocks _sharedInstance;

    public static QuestBlocks SharedInstance
    {
        get
        {
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    private void Start()
    {
        DesactivateBlock();
    }

    // Start is called before the first frame update
    public void DesactivateBlock()
    {
        questManager = FindObjectOfType<QuestManager>();

        Quest quest = questManager.QuestWithID(questID);

        if (!quest.gameObject.activeInHierarchy && quest.questCompleted)
        {
            Debug.Log("SE HA DESACTIVADO LA PORQUERIA DE BLOQUEO");
            gameObject.SetActive(false);
        }
    }
}
