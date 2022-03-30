using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float displaySpeed = 50;

    public bool isRunning { get; private set; }

    private Coroutine typingCoroutine;

    private readonly List<Punctuation> puncuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>(){'.', '!', '?'}, 0.6f),
        new Punctuation(new HashSet<char>(){',', ';', ':'}, 0.3f),
    };

    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        isRunning = true;
        textLabel.text = "";

        float t = 0;
        int charIndex = 0;

        while(charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;


            t += Time.deltaTime * displaySpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            //Loop through all characters typed since last frame
            for (int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                textLabel.text = textToType.Substring(0, charIndex);

                //Pause if it is a punctuation mark, not followed by a punctuation mark, and is not the last character
                if(isPunctuation(textToType[i], out float waitTime) && !isLast && !isPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        isRunning = false;
    }

    private bool isPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation p in puncuations)
        {
            if (p.Punctuations.Contains(character))
            {
                waitTime = p.WaitTime;
                return true;
            }
        }

        waitTime = 0;
        return false;
    }

    public void StopText()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
