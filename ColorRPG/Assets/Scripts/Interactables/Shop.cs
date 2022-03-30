using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        UIManager.instance.shopUIRef.SetActive(true);
        UIManager.instance.inventoryUIRef.SetActive(true);
        canInteract = false;
        FindObjectOfType<Player>().canMove = false;
    }
}
