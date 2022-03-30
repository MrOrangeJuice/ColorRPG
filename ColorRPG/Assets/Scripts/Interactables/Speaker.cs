using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Interactable
{
    public DialogueObject dialogue;


    public override void OnInteract()
    {
        base.OnInteract();

        DialogueUI.instance.ShowDialogue(dialogue);
        StartCoroutine(Reset());

        canInteract = false;
    }

    IEnumerator Reset()
    {
        yield return new WaitUntil(() => !DialogueUI.instance.dialogueShowing);
        canInteract = true;
    }
}
