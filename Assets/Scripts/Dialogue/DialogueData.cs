using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        public DialogueLine[] lines;
    }
}