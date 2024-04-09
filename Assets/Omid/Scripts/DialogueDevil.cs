using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDevil : MonoBehaviour
{
    public Text dialogText;
    public GameObject dialogBox;
    public GameObject panel;
    public GameObject NpcAvatar;
    public float typingSpeed = 0.05f;


    private Queue<string> sentences;
    private bool isTalking = false;
    private bool dialogStarted = false;
    private bool isTyping = false;

    void Start()
    {
        sentences = new Queue<string>();
        dialogBox.SetActive(false);
        NpcAvatar.SetActive(false);
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTalking == false && PlayerController.canTalk == true && dialogStarted == false)
        {
            StartDialog();
        }

        if (Input.GetMouseButtonDown(0) && isTalking)
        {
            DisplayNextSentence();
        }

        if (isTalking)
        {
            PlayerController.canMove = false;
        }
        else
        {
            PlayerController.canMove = true;
        }
        Debug.Log(sentences.Count);
    }

    void StartDialog()
    {
        dialogStarted = true;
        isTalking = true;
        dialogBox.SetActive(true);
        NpcAvatar.SetActive(true);
        panel.SetActive(true);

        sentences.Clear();
        LoadDialog();

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;

        // Display the name instantly in red color
        dialogText.text += "<color=red>Jabroni: </color>";

        foreach (char letter in sentences.Peek().ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Wait for typingSpeed seconds before typing the next character
        }
        isTyping = false;
    }

    void LoadDialog()
    {
        sentences.Enqueue("<color=red>Jabroni: </color>Well well, finally ready to pay your debt, bum?");
        sentences.Enqueue("Lets see...");
        sentences.Enqueue("As I recall, you owe me 290$ for that little... 'situation'...");
        sentences.Enqueue("But, I can see that you are very much broke");
        sentences.Enqueue("Well, you have to spend money to make money!");
        sentences.Enqueue("So, in the spirit of fairness... Here you go, 10$!");
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = sentences.Peek();
            isTyping = false;
        }
        else
        {
            if (sentences.Count == 1)
            {
                EndDialog();
                return;
            }

            dialogText.text = "";
            sentences.Dequeue();
            if (sentences.Count >= 1)
            {
                StartCoroutine(TypeSentence());
            }
        }
    }

    void EndDialog()
    {
        dialogBox.SetActive(false);
        NpcAvatar.SetActive(false);
        panel.SetActive(false);
        isTalking = false;
        dialogStarted = false;
    }
}

