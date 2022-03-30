using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Item item;

    public Image icon;
    public Text amountText;

    public void Start()
    {
        if (item != null)
        {
            icon.enabled = true;
            amountText.enabled = true;
            amountText.text = item.costInShop.ToString();
            icon.sprite = item.icon;
        }
    }

    public void BuyItem()
    {
        if (item == null)
        {
            return;
        }

        if (Inventory.instance.numOfCurrency >= item.costInShop)
        {
            UIManager.instance.itemToUse = item;
            UIManager.instance.SpawnBuyPrompt();
        }
        else
        {
            UIManager.instance.notEnoughCurrencyPromptRef.SetActive(true);

            //Prevent selling while attempting to buy
            if (UIManager.instance.sellItemPromptRef.activeSelf)
            {
                UIManager.instance.itemToUse = null;
                UIManager.instance.sellItemPromptRef.SetActive(false);
            }
        }
    }
}
