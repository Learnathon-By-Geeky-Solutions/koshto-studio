using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private Image speakerImage;
        [SerializeField] private TMP_Text speakerName;
        [SerializeField] private TMP_Text dialogueText;

        public void ShowDialogue(DialogueLine line)
        {
            dialogueBox.SetActive(true);
            speakerImage.sprite = line.speakerSprite;
            speakerName.text = line.speakerName;
            dialogueText.text = line.text;
        }

        public void HideDialogue()
        {
            dialogueBox.SetActive(false);
        }
        
        private void Start()
        {
            // Ensure it starts hidden
            HideDialogue();
        }
    }
}