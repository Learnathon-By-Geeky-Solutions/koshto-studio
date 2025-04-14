using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;

        private PlayerControls controls;
        private bool isPaused;

        public static event Action<bool> OnPauseToggled;

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Gameplay.Enable();
            controls.Gameplay.Pause.performed += HandlePauseInput;
        }

        private void OnDisable()
        {
            controls.Gameplay.Pause.performed -= HandlePauseInput;
            controls.Gameplay.Disable();
        }

        private void HandlePauseInput(InputAction.CallbackContext context)
        {
            Debug.Log($"Pause input performed. Phase: {context.phase}, Control: {context.control}");

            if (context.performed)
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
                ShowPauseMenu();
            else
                HidePauseMenu();

            Debug.Log("Pause triggered. isPaused: " + isPaused);
            OnPauseToggled?.Invoke(isPaused);
        }

        private void ShowPauseMenu()
        {
            Debug.Log("Showing Pause Menu");
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        private void HidePauseMenu()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void Resume() => HidePauseMenu();

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}