using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void CharacterItemSelectBack()
    {
        characterItemSelectRef.SetActive(false);
        itemToUse = null;
    }

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
