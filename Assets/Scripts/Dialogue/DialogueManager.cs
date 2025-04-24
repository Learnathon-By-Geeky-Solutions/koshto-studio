using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        private readonly Queue<DialogueLine> dialogueQueue = new();
        private bool isDialoguePlaying;

        public void StartDialogue(DialogueData dialogueData)
        {
            Time.timeScale = 0f;
            dialogueQueue.Clear();
            foreach (var line in dialogueData.lines)
                dialogueQueue.Enqueue(line);

            isDialoguePlaying = true;
            ShowNextLine();
        }

        private void Update()
        {
            if (isDialoguePlaying && Input.GetKeyDown(KeyCode.Return))
                ShowNextLine();
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