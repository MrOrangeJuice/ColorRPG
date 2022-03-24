using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public Equipment[] currentEquipment; //For now, will eventually make a dictionary to track each players inventory-

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

        //int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; --- Will use if we make multiple slots and then make a dictionary for color
        int numOfSlots = 3;
        currentEquipment = new Equipment[numOfSlots];
    }

    public void Equip(Equipment newItem, int colorIndex)
    {
        //int slotIndex = (int)newItem.equipmentSlot;

        Equipment oldItem = null;

        if (currentEquipment[colorIndex] != null)
        {
            oldItem = currentEquipment[colorIndex];
            inventory.Add(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[colorIndex] = newItem;

        UpdateEquipmentUI(colorIndex);
    }

    public void Unequip(int colorIndex)
    {
        if (currentEquipment[colorIndex] != null)
        {
            inventory.Add(currentEquipment[colorIndex]);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, currentEquipment[colorIndex]);
            }

            currentEquipment[colorIndex] = null;

            UpdateEquipmentUI(colorIndex);
        }

    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    public void UpdateEquipmentUI(int colorIndex)
    {
        if (UIManager.instance.equipmentMenuRef.activeSelf)
        {
            if (colorIndex == UIManager.instance.currentColorIndex)
            {
                if (currentEquipment[colorIndex] != null)
                {
                    UIManager.instance.equipmentSlot.AddItem(currentEquipment[colorIndex]);

                    UIManager.instance.equipmentCharacter.color = ColorMixer.MixColor(UIManager.instance.characterBaseColors[colorIndex], currentEquipment[colorIndex].colorToAdd,
                        1 - currentEquipment[colorIndex].colorWeight, currentEquipment[colorIndex].colorWeight);
                }
                else
                {
                    UIManager.instance.equipmentSlot.ClearSlot();
                    UIManager.instance.equipmentCharacter.color = UIManager.instance.characterBaseColors[colorIndex];
                }
            }
        }
    }
}
