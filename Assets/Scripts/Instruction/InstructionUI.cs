using TMPro;
using UnityEngine;

namespace InstructionSystem
{
    public class InstructionUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject instructionPanel;
        [SerializeField] private TMP_Text instructionText;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 3f;

        private void Start() => HideInstruction();

        public void ShowInstruction(string text)
        {
            instructionPanel.SetActive(true); // THIS WAS MISSING
            instructionText.text = text;

            if (displayDuration > 0)
                Invoke(nameof(HideInstruction), displayDuration);
        }

        public void HideInstruction()
        {
            instructionPanel.SetActive(false);
        }
    }
}