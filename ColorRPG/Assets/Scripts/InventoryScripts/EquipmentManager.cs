using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Equipment[] curentEquipment; //For now, will eventually make a dictionary to track each players inventory-

    private Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found");
            return;
        }

        instance = this;
    }

    public void Start()
    {
        inventory = Inventory.instance;

        int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        curentEquipment = new Equipment[numOfSlots];
    }

    public void Equip(Equipment newItem, string colorToUseOn)
    {
        int slotIndex = (int)newItem.equipmentSlot;

        Equipment oldItem = null;

        if (curentEquipment[slotIndex] != null)
        {
            oldItem = curentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        curentEquipment[slotIndex] = newItem;
    }

    public void Unequip(int slotIndex)
    {
        if (curentEquipment[slotIndex] != null)
        {
            inventory.Add(curentEquipment[slotIndex]);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, curentEquipment[slotIndex]);
            }

            curentEquipment[slotIndex] = null;
        }

    }

    public void UnequipAll()
    {
        for (int i = 0; i < curentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}
