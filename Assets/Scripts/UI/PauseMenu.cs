using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;
        private bool isPaused;
        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();
            controls.Gameplay.Pause.performed += ctx => TogglePause();
        }

        private void OnDisable()
        {
            controls.Gameplay.Pause.performed -= ctx => TogglePause();
            controls.Gameplay.Disable();
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
                Pause();
            else
                Resume();
        }
        void Start()
        {
            pauseMenuUI.SetActive(false);
        }


        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Game Paused");
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            Debug.Log("Game Resumed");
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
