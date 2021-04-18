﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    public GameObject Dialogue;

    DialogueTrigger dialogueTrigger;
    DialogueManager dialogueManager;

    GameObject player = null;

    bool end = false;
    bool canEnd = false;

    public GameObject tutorial;
    public GameObject myLight;
    public Dialogue dialogue;

    private void Start()
    {
        player = null;
        dialogueTrigger = Dialogue.GetComponent<DialogueTrigger>();
        dialogueManager = Dialogue.GetComponentInChildren<DialogueManager>();
        tutorial.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player != null)
        {
            if (tutorial.activeSelf)
            {
                StartCoroutine(StartTalking());
                GameManager.Instance.SetOnDialogue(true);
                dialogueManager.dialoguefinished = false;
            }
            else if (canEnd && !dialogueManager.typing)
                dialogueManager.DisplayNextSentence();
            else if (canEnd)
                dialogueManager.skip = true;
        }

        if (canEnd && !end && dialogueManager.IsDialogueFinished())
        {
            end = true;
            canEnd = false;

            Invoke("DialogueEnd", 0.5f);
        }
    }

    private void DialogueEnd()
    {
        GameManager.Instance.SetOnDialogue(false);
        GameManager.Instance.SetInputFreeze(false);
        myLight.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            player = coll.gameObject;
            tutorial.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            player = null;
            tutorial.SetActive(false);
        }
    }

    IEnumerator StartTalking()
    {
        tutorial.SetActive(false);
        Dialogue.SetActive(true);

        dialogueTrigger.dialogue = dialogue;

        GameManager.Instance.SetInputFreeze(true);
        GameManager.Instance.SetOnDialogue(true);
        Throw playerThrow = player.GetComponentInChildren<Throw>();
        playerThrow.StopCasting();

        yield return 0.1f;

        dialogueTrigger.TriggerDialogue();
        dialogueManager.typing = true;
        canEnd = true;
    }
}
