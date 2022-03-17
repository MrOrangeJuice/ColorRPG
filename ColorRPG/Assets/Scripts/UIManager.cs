using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //UI References
    public GameObject characterItemSelectRef;

    //Other Variables
    public Item itemToUse = null;

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

}
