using UnityEngine;

namespace DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueData dialogueData;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DialogueManager manager = FindObjectOfType<DialogueManager>();
                manager.StartDialogue(dialogueData);
                Destroy(gameObject); // Optional: trigger once
            }
        }
    }
}