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
    private bool initialD = false;
    private bool isTyping = false;

    void Start()
    {
        sentences = new Queue<string>();
        dialogBox.SetActive(false);
        NpcAvatar.SetActive(false);
        panel.SetActive(false);
        initialD = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTalking == false && PlayerController.canTalk == true)
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
    }

    void StartDialog()
    {
        isTalking = true;
        dialogBox.SetActive(true);
        NpcAvatar.SetActive(true);
        panel.SetActive(true);

        dialogText.text = "";
        sentences.Clear();
        if (initialD)
        {
            LoadInitialDialog();

        }
        if (!initialD)
        {
            LoadChitChatDialog();
        }
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;

        dialogText.text += "<color=red>Jabroni: </color>";

        foreach (char letter in sentences.Peek().ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void LoadChitChatDialog()
    {
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0:
                sentences.Enqueue("Here at Satan & Associates, we manage large amounts of capital at very reasonable interest rates.");
                sentences.Enqueue("If you would like to learn more, call 666-666-666");
                break;
            case 1:
                sentences.Enqueue("Maybe when you're done paying your debt, I can fix you up with a sub-prime mortgage loan...");
                break;
            case 2:
                sentences.Enqueue("Is that bench uncomfortable?");
                sentences.Enqueue("It looks uncomfortable");
                break;
        }
    }

    void LoadInitialDialog()
    {
        sentences.Enqueue("Well well, finally ready to pay your debt, bum?");
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
            dialogText.text = "<color=red>Jabroni: </color>";
            dialogText.text += sentences.Peek();
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
        initialD = false;
    }
}

