using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;

    public virtual void Use(string colorToUseOn)
    {
        Debug.Log("Use " + name + " on " + colorToUseOn);
        Inventory.instance.Remove(this);
    }
}
