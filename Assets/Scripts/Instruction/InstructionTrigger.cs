using UnityEngine;

namespace InstructionSystem
{
    public class InstructionTrigger : MonoBehaviour
    {
        [Header("Content")]
        [TextArea(3, 5)]
        [SerializeField] private string instructionText;

        [Header("Behavior")]
        [SerializeField] private bool oneTimeUse = true;
        [SerializeField] private bool requireKeyPress = false;

        private bool _hasTriggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hasTriggered && other.CompareTag("Player"))
            {
                if (!requireKeyPress)
                {
                    ShowInstruction();
                    DisableTriggerIfNeeded();
                }
            }
        }

        private void ShowInstruction()
        {
            var ui = FindObjectOfType<InstructionUI>();
            if (ui != null)
            {
                ui.ShowInstruction(instructionText);
            }
            else
            {
                Debug.LogError("InstructionUI not found in scene!");
            }
        }

        private void DisableTriggerIfNeeded()
        {
            if (oneTimeUse)
            {
                _hasTriggered = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}