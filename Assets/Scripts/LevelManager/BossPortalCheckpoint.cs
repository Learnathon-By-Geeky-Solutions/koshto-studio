using UnityEngine;
using System.Collections;
using Game;
using Scene;

namespace Level
{
    public class BossPortalCheckpoint : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private Transform player;
        [SerializeField] private InputManager inputManager;

        [Header("Scene References")]
        [SerializeField] private SceneFader sceneFader;

        [Header("Boss Arena Settings")]
        [SerializeField] private Transform bossArenaEntryPoint;
        [SerializeField] private GameObject virtualWalls;
        [SerializeField] private GameObject levelContents; // Add this field in Inspector

        private void Awake()
        {
            if (player == null)
                player = GameObject.FindWithTag("Player").transform;

            if (inputManager == null)
                inputManager = FindObjectOfType<InputManager>();

            if (sceneFader == null)
                sceneFader = FindObjectOfType<SceneFader>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            StartCoroutine(TransitionToBossArena());
        }

        private IEnumerator TransitionToBossArena()
        {
            // Disable player input
            inputManager.DisablePlayerInput();

            // Fade out
            yield return StartCoroutine(sceneFader.Fade(1f));

            // Move player to boss arena
            player.position = bossArenaEntryPoint.position;

            // Toggle level elements
            if (virtualWalls != null) virtualWalls.SetActive(true);
            if (levelContents != null) levelContents.SetActive(false);

            // Fade in
            yield return StartCoroutine(sceneFader.Fade(0f));

            // Re-enable player input
            inputManager.EnablePlayerInput();
        }
    }
}