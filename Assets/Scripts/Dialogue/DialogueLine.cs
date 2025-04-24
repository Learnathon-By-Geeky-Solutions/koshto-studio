namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public UnityEngine.Sprite speakerSprite;
        [UnityEngine.TextArea(2, 5)]
        public string text;
    }
}