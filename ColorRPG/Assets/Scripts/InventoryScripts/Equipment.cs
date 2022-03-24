using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{ 
    Weapon, 
    Shield
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    //public EquipmentSlot equipmentSlot;

    //Not Sure If We Need These
    public int armorModifier;
    public int damageModifier;

    //Color
    public Color colorToAdd;
    public float colorWeight;

    public override void Use(string colorToUseOn)
    {
        int index = -1;
        switch (colorToUseOn)
        {
            case "red":
                index = 0;
                break;
            case "yellow":
                index = 1;
                break;
            case "blue":
                index = 2;
                break;
            default:
                break;
        }

        EquipmentManager.instance.Equip(this, index);

        base.Use(colorToUseOn);
    }

}
