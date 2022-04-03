using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;

    public bool canInteract;

    public GameObject interactable;

    public GameObject dialogueBox;

    public DialogueManager man;

    GameObject player;

    public Dialogue dialogueAfter;
    public bool hasTalked;

    PlayerMovement4 move;


    private void Start()
    {
        dialogueBox.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

        move = FindObjectOfType<PlayerMovement4>();

    }

    private void Update()
    {
        if (canInteract)
        {
            /*if (player.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;

            }*/


            if (Input.GetKeyDown(KeyCode.E) && man.isDialogueRunning)
            {

                NextDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {

                dialogueBox.SetActive(true);
                TriggerDialogue();

            }


        }
    }

    public void TriggerDialogue()
    {
        if (!hasTalked)
        {
            man.StartDialogue(dialogue);
        }
        else
        {
            man.StartDialogue(dialogueAfter);
        }
    }

    public void NextDialogue()
    {
        man.DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            interactable.gameObject.SetActive(true);
            canInteract = true;


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            interactable.gameObject.SetActive(false);

            canInteract = false;

            man.sentences.Clear();

            man.EndDialogue();

            dialogueBox.SetActive(false);


        }

    }
}
