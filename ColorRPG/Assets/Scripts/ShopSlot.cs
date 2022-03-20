using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Item item;

    public Image icon;
    public Text amountText;

    public int costOfItem = 0;

    public void Start()
    {
        if (item != null)
        {
            icon.enabled = true;
            amountText.enabled = true;
            amountText.text = costOfItem.ToString();
            icon.sprite = item.icon;
        }
    }

    public void BuyItem()
    {
        if (Inventory.instance.numOfCurrency >= costOfItem && item != null)
        {
            Inventory.instance.numOfCurrency -= costOfItem;
            Inventory.instance.Add(item);
        }
        else
        {
            Debug.Log("Not enough currency");
        }
    }
}
