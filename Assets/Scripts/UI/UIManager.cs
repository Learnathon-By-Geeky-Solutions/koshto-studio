using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private PauseMenu pauseMenu;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ShowGameOverUI()
        {
            gameOverUI.Show();
        }

        public void HideGameOverUI()
        {
            gameOverUI.Hide();
        }

        public void ResumeGame()
        {
            pauseMenu.Resume();
        }
    }
}