using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public int space = 12;

    public Item healthPotionRef;
    public InventoryUI inventoryUI;

    public int numOfCurrency = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }

        instance = this;
    }

    public void Update()
    {
        //For Testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            Add(healthPotionRef);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            numOfCurrency += 1;
            inventoryUI.UpdateUI();
        }
    }

    /// <summary>
    /// Adds an item to the inventory if there is room. If one of this type already exists, simply update the counter.
    /// </summary>
    /// <param name="item">Item to add</param>
    public void Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room in inventory");
            return;
        }

        // Check if Item Exists
        for (int i = 0; i < items.Count; i++)
        {
            
            if (items[i].name == item.name)
            {
                //Update counter
                inventoryUI.UpdateCounter(i, 1);

                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }

                return;
            }
        }

        items.Add(item);

        //Update counter
        inventoryUI.UpdateCounter(items.Count - 1, 1);


        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    /// <summary>
    /// Remove one item from the inventory
    /// </summary>
    /// <param name="item">The item to remove</param>
    public void Remove(Item item)
    {
        // Check if Item Exists
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == item.name)
            {
                //Update counter
                inventoryUI.UpdateCounter(i, -1);

                if (onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }

                return;
            }
        }
    }

    /// <summary>
    /// Remove the item from the inventory
    /// </summary>
    /// <param name="item">The item to remove</param>
    public void RemoveAll(Item item)
    {
        items.Remove(item);

        //Call any events subscribed to OnItemChanged
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

}
