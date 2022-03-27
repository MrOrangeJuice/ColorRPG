using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueText
{
    [TextArea] public string[] dialogue;
    //public bool isPlayer;
    //public string name;
    //public Sprite sprite;
}


[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    //public List<DialogueText> dialogues;
    [TextArea] public string[] dialogue;
}
