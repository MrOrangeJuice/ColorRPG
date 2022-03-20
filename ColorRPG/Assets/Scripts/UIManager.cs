using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //UI References
    public GameObject characterItemSelectRef;
    public GameObject sellItemPromptRef;
    public GameObject shopUIRef;
    public GameObject inventoryUIRef;

    //Other Variables
    [HideInInspector]public Item itemToUse = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of UIManager found");
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        //Scene Switch For Testing
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Inputs for Opening UI

        //Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUIRef.SetActive(!inventoryUIRef.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            shopUIRef.SetActive(!shopUIRef.activeSelf);
        }
    }

    public void SpawnSellPrompt()
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to sell");
            return;
        }

        sellItemPromptRef.SetActive(true);

        sellItemPromptRef.GetComponentInChildren<Text>().text = "Sell item for " + itemToUse.amountSoldFor + "?";
    }

    /// <summary>
    /// Backs out of the character item select 
    /// </summary>
    public void CharacterItemSelectBack()
    {
        characterItemSelectRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Backs out of the sell item prompt
    /// </summary>
    public void SellItemPromptBack()
    {
        sellItemPromptRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Triggers the item's effect on the character selected.
    /// </summary>
    /// <param name="color"></param>
    public void UseItemOnCharacter(string color)
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to use");
            return;
        }

        itemToUse.Use(color);

        characterItemSelectRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Sells the item to the shop
    /// </summary>
    public void SellItem()
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to sell");
            return;
        }

        itemToUse.Sell();

        sellItemPromptRef.SetActive(false);
        itemToUse = null;
    }

}
