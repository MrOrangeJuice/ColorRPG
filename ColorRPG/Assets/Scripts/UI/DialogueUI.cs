using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    [SerializeField] private DialogueObject testDialogue;

    [SerializeField] private float fadeTime = .5f;

    //Get access to UI elements
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    //[SerializeField] private TMP_Text nameText;
    //[SerializeField] private Image playerSprite;
    //[SerializeField] private Image characterSprite;

    //private Image[] imageRefs;
    //private TMP_Text[] textRefs;

    private TypewriterEffect typewriterEffect;

    public bool dialogueShowing = false;

    public Player playerRef;

    private void Awake()
    {
        instance = this;
        playerRef = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //textRefs = transform.GetComponentsInChildren<TMP_Text>();
        //imageRefs = transform.GetComponentsInChildren<Image>();
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
        //ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueShowing = true;

        textLabel.DOFade(1, fadeTime);
        dialogueBox.GetComponent<Image>().DOFade(.4f, fadeTime);

        //playerSprite.enabled = false;
        //characterSprite.enabled = false;

        playerRef.canMove = false;

        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        //foreach (DialogueText dt in dialogueObject.dialogues)
        //{
        //    //ChangeSpeaker(dt.isPlayer, dt.sprite, dt.name);
        //
        //    foreach (string dialogue in dt.dialogue)
        //    {
        //        yield return RunTypingEffect(dialogue);
        //        textLabel.text = dialogue;
        //        yield return null;
        //        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
        //    }   
        //}

        foreach (string dialogue in dialogueObject.dialogue)
        {
            yield return RunTypingEffect(dialogue);
            textLabel.text = dialogue;
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0));
        }

        //Fade out the dialogue box
        //playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        //characterSprite.color = new Color(characterSprite.color.r, characterSprite.color.g, characterSprite.color.b, 1f);

        //foreach (Image image in imageRefs)
        //{
        //    image.DOFade(0, fadeTime);
        //}

        //yield return new WaitUntil(() => playerSprite.color.a <= 0);
        textLabel.DOFade(0, fadeTime);
        dialogueBox.GetComponent<Image>().DOFade(0, fadeTime).OnComplete(CloseDialogueBox);

        //CloseDialogueBox();
    }

    public IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while(typewriterEffect.isRunning)
        {
            yield return null;

            if (Input.GetMouseButtonDown(0))
            {
                typewriterEffect.StopText();
            }
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = "";

        playerRef.canMove = true;

        dialogueShowing = false;
    }

    //public void ChangeSpeaker(bool isPlayer, Sprite sprite, string name)
    //{
    //    if (isPlayer)
    //    {
    //        if (!playerSprite.enabled)
    //        {
    //            playerSprite.enabled = true;
    //        }
    //
    //        playerSprite.sprite = sprite;
    //        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
    //        characterSprite.color = new Color(characterSprite.color.r, characterSprite.color.g, characterSprite.color.b, .5f);
    //    }
    //    else
    //    {
    //        if (!characterSprite.enabled)
    //        {
    //            characterSprite.enabled = true;
    //        }
    //
    //        characterSprite.sprite = sprite;
    //        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, .5f);
    //        characterSprite.color = new Color(characterSprite.color.r, characterSprite.color.g, characterSprite.color.b, 1f);
    //    }
    //
    //    nameText.text = name;
    //}
}
