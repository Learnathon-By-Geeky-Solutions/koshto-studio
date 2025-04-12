namespace UI
{
    using UnityEngine;

    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] 
        private GameObject panel; // Field is now private

        public GameObject Panel // Public property to access the private field
        {
            get => panel;
            set => panel = value;
        }

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);
    }
}
