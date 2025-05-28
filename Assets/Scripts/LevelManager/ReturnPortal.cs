using UnityEngine;
using System.Collections;
using Game;
using Scene;

namespace Level
{
    public class ReturnPortal : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private InputManager inputManager;

        [Header("Return Settings")]
        [SerializeField] private Transform levelReturnPoint;
        [SerializeField] private GameObject virtualWalls;
        [SerializeField] private GameObject levelContents; // Add this field in Inspector

        private void Awake()
        {
            if (player == null)
                player = GameObject.FindWithTag("Player").transform;

            if (sceneFader == null)
                sceneFader = FindObjectOfType<SceneFader>();

            if (inputManager == null)
                inputManager = FindObjectOfType<InputManager>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            StartCoroutine(ReturnToLevel());
        }

        private IEnumerator ReturnToLevel()
        {
            inputManager.DisablePlayerInput();

            // Fade out
            yield return StartCoroutine(sceneFader.Fade(1f));

            // Move player back
            player.position = levelReturnPoint.position;

            // Toggle level elements
            if (virtualWalls != null) virtualWalls.SetActive(false);
            if (levelContents != null) levelContents.SetActive(true);

            // Fade in
            yield return StartCoroutine(sceneFader.Fade(0f));

            inputManager.EnablePlayerInput();
        }
    }
}