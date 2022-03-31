using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "Inventory/Currency")]
public class AddCurrency : Item
{
    public int currencyAmount = 5;

    public override void onAdd()
    {
        Inventory.instance.numOfCurrency += currencyAmount;
        Inventory.instance.Remove(this);
    }

    /// <summary>
    /// Item's effect on use
    /// </summary>
    /// <param name="colorToUseOn">The character that it will affect</param>
    public override void Use(string colorToUseOn)
    {
        
    }

    /// <summary>
    /// Removes the item from inventory and updates the player's currency
    /// </summary>
    public override void Sell()
    {
        
    }

    public override void Buy()
    {
        
    }
}
