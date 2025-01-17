using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private Queue<string> sentences;
    [SerializeField] private TextMeshProUGUI text_box;

    private CanvasGroup dialogue_box;
    private bool in_dialogue = false;

    private IEnumerator text_printer;
    private bool text_running = false;

    private bool doing_action = false;
    private PuzzleAction doing;

    private void Awake()
    {
        instance = this;
        sentences = new Queue<string>();
        dialogue_box = GetComponent<CanvasGroup>();
    }

    public void startDialogue(string[] dialogue)
    {
        if (in_dialogue) return;

        GameManager.instance.disableInputs();
        sentences.Clear();
        StartCoroutine(fadeInDialogue());
        in_dialogue = true;
        //lock player movement

        //load all dialogue into the queue
        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void startDialogue(string[] dialogue, PuzzleAction _do)
    {
        if (in_dialogue) return;

        GameManager.instance.disableInputs();
        sentences.Clear();
        StartCoroutine(fadeInDialogue());
        in_dialogue = true;
        //lock player movement

        //load all dialogue into the queue
        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        doing = _do;
        doing_action = true;
        DisplayNextSentence(_do);
    }

    private void DisplayNextSentence()
    {
        SFXManager1.instance.playDialogueSFX();
        if (sentences.Count == 0)
        {
            StartCoroutine(fadeOutDialogue());
            in_dialogue = false;
            GameManager.instance.enableInputs();
            //unlock player movement
            return;
        }

        if (text_running)
        {
            StopCoroutine(text_printer);
            text_printer = displaySentence(sentences.Dequeue());
            StartCoroutine(text_printer);
        }
        else
        {
            text_printer = displaySentence(sentences.Dequeue());
            StartCoroutine(text_printer);
            text_running = true;
        }
    }

    private void DisplayNextSentence(PuzzleAction _do)
    {
        SFXManager1.instance.playDialogueSFX();
        if (sentences.Count == 0)
        {
            StartCoroutine(fadeOutDialogue());
            in_dialogue = false;
            GameManager.instance.enableInputs();
            //unlock player movement
            Debug.Log("trying " + _do.ToString() + " action");
            _do.action();
            doing_action = false;
            doing = null;
            return;
        }

        if (text_running)
        {
            StopCoroutine(text_printer);
            text_printer = displaySentence(sentences.Dequeue());
            StartCoroutine(text_printer);
        }
        else
        {
            text_printer = displaySentence(sentences.Dequeue());
            StartCoroutine(text_printer);
            text_running = true;
        }
    }

    private IEnumerator displaySentence(string text)
    {
        text_box.text = "";        
        foreach (char c in text.ToCharArray())
        {
            text_box.text += c;
            yield return null;
        }

        text_running = false;
    }

    private IEnumerator fadeInDialogue()
    {
        for (float i = 0; i <= 1; i += 0.2f)
        {
            dialogue_box.alpha = i;
            yield return null;
        }
    }

    private IEnumerator fadeOutDialogue()
    {
        for (float i = 1; i >= 0; i -= 0.2f)
        {
            dialogue_box.alpha = i;
            yield return null;
        }
    }

    private void Update()
    {
        //if in dialogue and player  left clicks
        if (in_dialogue && (Input.GetMouseButtonDown(0)))
        {
            if (doing_action)
            {
                DisplayNextSentence(doing);
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }
}
