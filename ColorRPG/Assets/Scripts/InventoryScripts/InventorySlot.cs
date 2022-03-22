using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;

    public Image icon;
    public Text amountText;

    public int numOfItem = 0; 

    /// <summary>
    /// Add a item to the slot
    /// </summary>
    /// <param name="newItem">The item to add</param>
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.enabled = true;
    }

    /// <summary>
    /// Clears the slot of items
    /// </summary>
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        amountText.enabled = false;
    }

    /// <summary>
    /// Use the item in this slot
    /// </summary>
    public void UseItem()
    {
        if (item != null)
        {
            UIManager.instance.itemToUse = item;

            //If Shop is Open Prompt Player to Sell Item
            if (UIManager.instance.shopUIRef.activeSelf)
            {
                UIManager.instance.SpawnSellPrompt();
            }
            else
            {
                UIManager.instance.characterItemSelectRef.SetActive(true);            
            }

        }
    }
}
