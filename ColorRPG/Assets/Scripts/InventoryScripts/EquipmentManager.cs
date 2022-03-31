using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    public Color currentTheme;

    public Equipment[] currentEquipment;
    public Color[] currentColors;

    private Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    public int[] playerHealth = new int[3];
    public string[] playerNames = new string[] { "red", "yellow", "blue" };
    public int maxHealth = 25;

    private Player playerRef;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found");
            return;
        }

        instance = this;

        playerRef = FindObjectOfType<Player>();
    }

    public void Start()
    {
        currentColors = new Color[3];

        for (int i = 0; i < currentColors.Length; i++)
        {
            currentColors[i] = UIManager.instance.CharacterBaseColors[i];
        }

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

            UIManager.instance.itemDescriptionRef.SetActive(false);
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

                    Color temp = UIManager.instance.CharacterBaseColors[colorIndex];

                     currentColors[colorIndex] = ColorMixer.MixColor(temp, currentEquipment[colorIndex].colorToAdd,
                        1 - currentEquipment[colorIndex].colorWeight, currentEquipment[colorIndex].colorWeight);

                    UIManager.instance.equipmentCharacter.color = currentColors[colorIndex];
                }
                else
                {
                    UIManager.instance.equipmentSlot.ClearSlot();
                    currentColors[colorIndex] = UIManager.instance.CharacterBaseColors[colorIndex];
                    UIManager.instance.equipmentCharacter.color = currentColors[colorIndex];
                }
            }
        }

        playerRef.sr.color = currentColors[0];
    }

    public void Rest()
    {
        for (int i = 0; i < playerHealth.Length; i++)
        {
            playerHealth[i] = maxHealth;
        }
    }
}
