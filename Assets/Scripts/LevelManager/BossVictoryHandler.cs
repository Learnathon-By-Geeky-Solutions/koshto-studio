using UnityEngine;
using System.Collections;
using Game;
using Scene;

namespace Level
{
    public class BossVictoryHandler : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private Transform player; // Drag player in Inspector
        [SerializeField] private bool disablePlayerDuringSequence = true;

        [Header("Return Settings")]
        [SerializeField] private Transform returnPoint; // Create empty GameObject for return position
        [SerializeField] private float returnDelay = 1.0f;

        [Header("Effects")]
        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private GameObject victoryEffects;
        [SerializeField] private GameObject bossArenaContents; // Optional: hide boss arena

        [Header("Win Screen")]
        [SerializeField] private GameObject winScreen; // Assign your win screen UI panel
        [SerializeField] private float winScreenDuration = 3f;
        [SerializeField] private bool showWinScreenBeforeReturn = true;

        private InputManager inputManager;

        private void Awake()
        {
            // Fallback if not set in Inspector
            if (player == null)
                player = GameObject.FindWithTag("Player").transform;

            if (sceneFader == null)
                sceneFader = FindObjectOfType<SceneFader>();

            inputManager = FindObjectOfType<InputManager>();
        }

        public void OnBossDefeated()
        {
            StartCoroutine(VictorySequence());
        }

        private IEnumerator VictorySequence()
        {
            // Disable player input if needed
            if (disablePlayerDuringSequence && inputManager != null)
                inputManager.DisablePlayerInput();

            // Play victory effects
            if (victoryEffects != null)
                victoryEffects.SetActive(true);

            // Show win screen if configured
            if (showWinScreenBeforeReturn && winScreen != null)
            {
                winScreen.SetActive(true);
                yield return new WaitForSeconds(winScreenDuration);
                winScreen.SetActive(false);
            }

            yield return new WaitForSeconds(returnDelay);

            // Fade out
            yield return StartCoroutine(sceneFader.Fade(1f));

            // Teleport player
            if (returnPoint != null)
            {
                player.position = returnPoint.position;
            }

            // Hide boss arena if needed
            if (bossArenaContents != null)
                bossArenaContents.SetActive(false);

            // Fade in
            yield return StartCoroutine(sceneFader.Fade(0f));

            // Show win screen if configured to show after return
            if (!showWinScreenBeforeReturn && winScreen != null)
            {
                winScreen.SetActive(true);
                yield return new WaitForSeconds(winScreenDuration);
                winScreen.SetActive(false);
            }

            // Re-enable input if disabled
            if (disablePlayerDuringSequence && inputManager != null)
                inputManager.EnablePlayerInput();
        }
    }
}