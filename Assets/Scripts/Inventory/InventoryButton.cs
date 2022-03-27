using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public enum ItemType { WEAPON = 0, ITEM = 1, SPECIAL_ITEM = 2 };
    // Recordemos: WEAPON = 0, ITEM = 1, ARMOR = 2 y RING = 3

    public int itemIdx; //item index
    public ItemType type;

    // Ahora cada botón tendrá la información asociada a él, no como antes que no servía porque el indice era i = 3 siempre
    // en la clase UIManager, método FillInventory(), que ya no aparece, claro...
    public void ActivateButton()
    {
        switch (type)
        {
            // En el caso que tengamos un botón de tipo arma
            case ItemType.WEAPON:
                FindObjectOfType<WeaponManager>().ChangeWeapon(itemIdx);
                break;

            case ItemType.ITEM:
                Debug.Log("Consumir item (pendiente)");
                break;

                //case ItemType.ARMOR:
                //    Debug.Log("FUTURO DLC :v");
                //    break;

                //case ItemType.RING:
                //    Debug.Log("FUTURO DLC :v");
                //    break;


        }
        ShowDescription();
    }

    private void OnMouseOver()
    {
        ShowDescription();
    }

    public void ShowDescription()
    {
        string description = "";

        switch (type)
        {
            // En el caso que tengamos un botón de tipo arma
            case ItemType.WEAPON:
                description = FindObjectOfType<UIManager>().inventoryDescription.text = FindObjectOfType<WeaponManager>().GetWeaponAt(itemIdx).swordName;
                break;

            case ItemType.ITEM:
                description = "Item Consumible";
                break;

            //case ItemType.ARMOR:
            //    Debug.Log("FUTURO DLC :v");
            //    break;

            //case ItemType.RING:
            //    Debug.Log("FUTURO DLC :v");
            //    break;

            case ItemType.SPECIAL_ITEM:
                QuestItem item = FindObjectOfType<ItemsManager>().GetItemAt(itemIdx);
                description = item.itemName;
                break;
        }
        FindObjectOfType<UIManager>().inventoryDescription.text = description;
    }

    public void ClearDescription()
    {
        FindObjectOfType<UIManager>().inventoryDescription.text = "";
    }
}
