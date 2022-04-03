using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public bool isDialogueRunning;
	//public Text nameText;
	public Text dialogueText;

	public Animator animator;

	[HideInInspector]
	public Queue<string> sentences;

	[SerializeField] NPC npc;

	PlayerMovement4 playerMove;

	public int indexToTakeOff;

	Game game;

	public int indexOfArmor;
	public bool isDoubleArmor;

	public bool isDziad;


	// Use this for initialization
	void Start()
	{
		game = FindObjectOfType<Game>();
		playerMove = FindObjectOfType<PlayerMovement4>();
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		playerMove.isTalking = true;
		playerMove.canMove = false;
		isDialogueRunning = true;

		animator.SetBool("IsOpen", true);

		//nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(!npc.hasTalked && sentences.Count == indexToTakeOff)
        {
			game.TakeOffArmor(indexOfArmor, isDoubleArmor);
        }

		if (sentences.Count == 0)
		{
			npc.hasTalked = true;
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		isDialogueRunning = false;
		animator.SetBool("IsOpen", false);

		playerMove.canMove = true;
		playerMove.isTalking = false;

		if (isDziad)
        {
			playerMove.isAbleToDash = true;
		}
		else
        {
			playerMove.isAbleToWallJump = true;
        }

	}

}