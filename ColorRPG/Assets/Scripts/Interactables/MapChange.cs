using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChange : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        canInteract = false;

        UIManager.instance.CloseAllMenus();
        UIManager.instance.adventurePromptRef.SetActive(true);

        FindObjectOfType<Player>().canMove = false;
    }
}
