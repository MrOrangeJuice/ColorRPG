using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public Item itemInside;
    public DialogueObject dialogue;
    public Sprite openSprite;
    private bool open = false;


    public override void OnInteract()
    {
        if (open)
        {
            return;
        }

        base.OnInteract();

        canInteract = false;
        open = true;

        DialogueObject newDialogue = new DialogueObject();
        newDialogue.dialogue = new string[dialogue.dialogue.Length];

        //Update dialogue to show item name
        for (int i = 0; i < dialogue.dialogue.Length; i++)
        {
            newDialogue.dialogue[i] = dialogue.dialogue[i].Replace("ITEM", itemInside.name);
        }

        DialogueUI.instance.ShowDialogue(newDialogue);
        Inventory.instance.Add(itemInside);

        GetComponent<SpriteRenderer>().sprite = openSprite;
    }
}
