using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : Interactable
{
    // Start is called before the first frame update
    public override void OnInteract()
    {
        base.OnInteract();

        UIManager.instance.restPromptRef.SetActive(true);
        canInteract = false;
        FindObjectOfType<Player>().canMove = false;
    }
}
