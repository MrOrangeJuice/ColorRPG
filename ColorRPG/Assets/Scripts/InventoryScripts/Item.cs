using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int amountSoldFor = 1;
    public int costInShop = 5;

    /// <summary>
    /// Item's effect on use
    /// </summary>
    /// <param name="colorToUseOn">The character that it will affect</param>
    public virtual void Use(string colorToUseOn)
    {
        Debug.Log("Use " + name + " on " + colorToUseOn);
        Inventory.instance.Remove(this);
    }

    /// <summary>
    /// Removes the item from inventory and updates the player's currency
    /// </summary>
    public virtual void Sell()
    {
        Inventory.instance.numOfCurrency += amountSoldFor;
        Inventory.instance.Remove(this);
    }

    public virtual void Buy()
    {
        Inventory.instance.numOfCurrency -= costInShop;
        Inventory.instance.Add(this);
    }
}
