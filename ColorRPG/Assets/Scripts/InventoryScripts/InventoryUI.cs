using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    private Inventory inventory;

    private InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        //Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    /// <summary>
    /// Adds or Clears Item Slots As Necessary
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    /// <summary>
    /// Updates the items counter
    /// </summary>
    /// <param name="index">the index of the item in the items list</param>
    /// <param name="numToAdd">the number to add to the counter</param>
    public void UpdateCounter(int index, int numToAdd)
    {
        slots[index].numOfItem += numToAdd;
        slots[index].amountText.text = slots[index].numOfItem.ToString();

        if (slots[index].numOfItem <= 0)
        {
            inventory.RemoveAll(inventory.items[index]);
        }
    }
}
