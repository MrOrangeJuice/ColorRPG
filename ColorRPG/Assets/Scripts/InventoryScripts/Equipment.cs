using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Head, 
    Chest, 
    Legs, 
    Weapon, 
    Shield, 
    Feet
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;

    //Not Sure If We Need These
    public int armorModifier;
    public int damageModifier;

    public override void Use(string colorToUseOn)
    {
        EquipmentManager.instance.Equip(this, colorToUseOn);

        base.Use(colorToUseOn);
    }

}
