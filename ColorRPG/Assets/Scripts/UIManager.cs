using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //UI References
    #region UI GameObject References
    public GameObject characterItemSelectRef;
    public GameObject sellItemPromptRef;
    public GameObject buyItemPromptRef;
    public GameObject notEnoughCurrencyPromptRef;
    public GameObject shopUIRef;
    public GameObject inventoryUIRef;
    public GameObject pauseMenuRef;
    public GameObject optionsMenuRef;
    public GameObject adventurePromptRef;
    public GameObject restPromptRef;
    public GameObject restResponseRef;
    public GameObject townMenuRef;
    #endregion

    //Other Variables
    [HideInInspector]public Item itemToUse = null;
    public bool paused;
    public bool townScene = true;
    public int RestCost = 10;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of UIManager found");

            if (this != instance)
            {
                Destroy(gameObject);
            }
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        CloseAllMenus();

        if (townScene)
        {
            townMenuRef.SetActive(true);
        }
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
        if (Input.GetKeyDown(KeyCode.I) && !paused && townScene)
        {
            Btn_InventoryToggle();
        }

        if (Input.GetKeyDown(KeyCode.S) && !paused && townScene)
        {
            Btn_ShopToggle();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuRef.activeSelf && townScene)
        {
            optionsMenuRef.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && townScene)
        {
            PauseMenuToggle();
        }
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
    /// Pops up the sell prompt when button is clicked
    /// </summary>
    public void SpawnSellPrompt()
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to sell");
            return;
        }

        sellItemPromptRef.SetActive(true);

        //Prevent buying and selling at the same time
        if (buyItemPromptRef.activeSelf)
        {
            buyItemPromptRef.SetActive(false);
        }

        if (notEnoughCurrencyPromptRef.activeSelf)
        {
            notEnoughCurrencyPromptRef.SetActive(false);
        }

        sellItemPromptRef.GetComponentInChildren<Text>().text = "Sell item for " + itemToUse.amountSoldFor + "?";
    }

    /// <summary>
    /// Pops up the buy prompt when button is clicked
    /// </summary>
    public void SpawnBuyPrompt()
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to buy");
            return;
        }

        buyItemPromptRef.SetActive(true);

        //Prevent buying and selling at the same time
        if (sellItemPromptRef.activeSelf)
        {
            sellItemPromptRef.SetActive(false);
        }

        buyItemPromptRef.GetComponentInChildren<Text>().text = "Buy item for " + itemToUse.amountSoldFor + "?";
    }

    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
    public void PauseMenuToggle()
    {
        pauseMenuRef.SetActive(!pauseMenuRef.activeSelf);
        paused = pauseMenuRef.activeSelf;
    }

    /// <summary>
    /// Helper function to cloase all menus except town
    /// </summary>
    public void CloseAllMenus()
    {
        characterItemSelectRef.SetActive(false);
        sellItemPromptRef.SetActive(false);
        buyItemPromptRef.SetActive(false);
        notEnoughCurrencyPromptRef.SetActive(false);
        adventurePromptRef.SetActive(false);
        restPromptRef.SetActive(false);
        restResponseRef.SetActive(false);
        shopUIRef.SetActive(false);
        inventoryUIRef.SetActive(false);
        pauseMenuRef.SetActive(false);
        optionsMenuRef.SetActive(false);
        townMenuRef.SetActive(false);

        paused = false;
    }


    #region Button Methods

    /// <summary>
    /// Backs out of the character item select 
    /// </summary>
    public void Btn_CharacterItemSelectBack()
    {
        characterItemSelectRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Backs out of the sell item prompt
    /// </summary>
    public void Btn_SellItemPromptBack()
    {
        sellItemPromptRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Backs out of the buy item prompt
    /// </summary>
    public void Btn_BuyItemPromptBack()
    {
        buyItemPromptRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Backs out of the not enough currency promt
    /// </summary>
    public void Btn_CurrencyPromptBack()
    {
        notEnoughCurrencyPromptRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Sells the item to the shop
    /// </summary>
    public void Btn_SellItem()
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

    /// <summary>
    /// Buys the item from the shop
    /// </summary>
    public void Btn_BuyItem()
    {
        if (itemToUse == null)
        {
            Debug.LogError("No item set to buy");
            return;
        }

        itemToUse.Buy();

        buyItemPromptRef.SetActive(false);
        itemToUse = null;
    }

    /// <summary>
    /// Toggles the options menu
    /// </summary>
    public void Btn_ToggleOptionsMenu()
    {
        optionsMenuRef.SetActive(!optionsMenuRef.activeSelf);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Btn_QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Toggles the town menu
    /// </summary>
    public void Btn_TownToggle()
    {
        townMenuRef.SetActive(!townMenuRef.activeSelf);

        //If Town Menu is active, close other menus
        if (townMenuRef.activeSelf)
        {
            inventoryUIRef.SetActive(false);
            shopUIRef.SetActive(false);
        }
    }

    /// <summary>
    /// Toogles the adventure prompt
    /// </summary>
    public void Btn_AdventureSelection()
    {
        adventurePromptRef.SetActive(!adventurePromptRef.activeSelf);
        townMenuRef.SetActive(!adventurePromptRef.activeSelf);
    }

    /// <summary>
    /// Changes the scene
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void Btn_LoadScene(string sceneName)
    {
        CloseAllMenus();

        if (sceneName == "DestinyScene")
        {
            townScene = true;
            townMenuRef.SetActive(true);
        }
        else
        {
            townScene = false;
        }

        paused = false;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Toggles the inventory
    /// </summary>
    public void Btn_InventoryToggle()
    {
        inventoryUIRef.SetActive(!inventoryUIRef.activeSelf);

        //If Inventory and shop are closed, spawn town menu
        if (!inventoryUIRef.activeSelf && !shopUIRef.activeSelf)
        {
            townMenuRef.SetActive(true);
        }
        //Otherwise close the town menu
        else
        {
            townMenuRef.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the Shop
    /// </summary>
    public void Btn_ShopToggle()
    {
        inventoryUIRef.SetActive(!shopUIRef.activeSelf);
        shopUIRef.SetActive(!shopUIRef.activeSelf);

        //If Shop is closed, spawn town menu
        if (!shopUIRef.activeSelf)
        {
            townMenuRef.SetActive(true);
        }
        //Otherwise close the town menu
        else
        {
            townMenuRef.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the rest prompt
    /// </summary>
    public void Btn_RestPrompt()
    {
        restPromptRef.SetActive(!restPromptRef.activeSelf);
        townMenuRef.SetActive(!restPromptRef.activeSelf);
    }

    /// <summary>
    /// Toggle the rest response prompt and check if player has enough currency
    /// </summary>
    public void Btn_RestResponsePrompt()
    {
        restResponseRef.SetActive(!restResponseRef.activeSelf);

        if (restResponseRef.activeSelf)
        {
            restPromptRef.SetActive(false);

            if (Inventory.instance.numOfCurrency >= RestCost)
            {
                Inventory.instance.numOfCurrency -= RestCost;
                GetComponent<InventoryUI>().UpdateUI();

                restResponseRef.GetComponentInChildren<Text>().text = "Your party has fully rested!";

                //Heal Players
            }
            else
            {
                restResponseRef.GetComponentInChildren<Text>().text = "Not enough currency to rest.";
            }
        }
        else
        {
            townMenuRef.SetActive(true);
        }

    }
    #endregion

}
