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
    public GameObject pauseMenuRef;
    public GameObject optionsMenuRef;

    //Other Variables
    [HideInInspector]public Item itemToUse = null;
    public bool paused;
    public bool gameScene = true;

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
        if (Input.GetKeyDown(KeyCode.I) && !paused && gameScene)
        {
            inventoryUIRef.SetActive(!inventoryUIRef.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.S) && !paused && gameScene)
        {
            shopUIRef.SetActive(!shopUIRef.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuRef.activeSelf && gameScene)
        {
            optionsMenuRef.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameScene)
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

        sellItemPromptRef.GetComponentInChildren<Text>().text = "Sell item for " + itemToUse.amountSoldFor + "?";
    }

    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>
    public void PauseMenuToggle()
    {
        pauseMenuRef.SetActive(!pauseMenuRef.activeSelf);
        paused = pauseMenuRef.activeSelf;
    }

    public void CloseAllMenus()
    {
        characterItemSelectRef.SetActive(false);
        sellItemPromptRef.SetActive(false);
        shopUIRef.SetActive(false);
        inventoryUIRef.SetActive(false);
        pauseMenuRef.SetActive(false);
        optionsMenuRef.SetActive(false);

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
    /// Toggles the options menu
    /// </summary>
    public void Btn_ToggleOptionsMenu()
    {
        optionsMenuRef.SetActive(!optionsMenuRef.activeSelf);
    }

    /// <summary>
    /// Goes back to the MainMenu scene
    /// </summary>
    public void Btn_MainMenu()
    {
        CloseAllMenus();
        gameScene = false;
        paused = false;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Btn_QuitGame()
    {
        Application.Quit();
    }
    #endregion

}
