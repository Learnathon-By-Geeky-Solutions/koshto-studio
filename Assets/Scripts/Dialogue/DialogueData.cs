using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private DialogueLine[] lines;

        public DialogueLine[] Lines => lines;
    }
}
