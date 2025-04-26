using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        private readonly Queue<DialogueLine> dialogueQueue = new();
        private bool isDialoguePlaying;

        private PlayerControls playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            playerControls.Enable();
            playerControls.Gameplay.AdvanceDialogue.performed += OnAdvanceDialogue;
        }

        private void OnDisable()
        {
            playerControls.Gameplay.AdvanceDialogue.performed -= OnAdvanceDialogue;
            playerControls.Disable();
        }

        public void StartDialogue(DialogueData dialogueData)
        {
            Time.timeScale = 0f;
            dialogueQueue.Clear();
            foreach (var line in dialogueData.lines)
                dialogueQueue.Enqueue(line);

            isDialoguePlaying = true;
            ShowNextLine();
        }

        private void OnAdvanceDialogue(InputAction.CallbackContext _)
        {
            if (isDialoguePlaying)
            {
                ShowNextLine();
            }
        }

        private void ShowNextLine()
        {
            if (dialogueQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            var nextLine = dialogueQueue.Dequeue();
            dialogueUI.ShowDialogue(nextLine);
        }

        private void EndDialogue()
        {
            Time.timeScale = 1f;
            isDialoguePlaying = false;
            dialogueUI.HideDialogue();
        }
    }
}