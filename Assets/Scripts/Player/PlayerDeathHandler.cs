using UnityEngine;
using Common;
using System.Collections;
using UI;
using Game;
using Player.Weapons;

namespace Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        private Player.Input.PlayerController playerController;
        private WeaponHandler weaponHandler;
        private InputManager inputManager;

        [SerializeField] private GameObject deathScreenUI;
        private Animator animator;
        private Health health;
        private GameOverUI gameOverUI;
        private Rigidbody2D rb;

        [SerializeField] private float respawnDelay = 2f;

        private bool isDead = false; // 🔐 Prevent duplicate triggers

        private void Awake()
        {
            playerController = GetComponent<Player.Input.PlayerController>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody2D>();
            weaponHandler = GetComponent<WeaponHandler>();
            inputManager = FindObjectOfType<InputManager>();
            gameOverUI = FindObjectOfType<GameOverUI>();

            health.OnDeath += HandleDeath;
        }

        private void HandleDeath()
        {
            if (isDead) return; // ✅ Prevent multiple deaths from stacking

            isDead = true;

            animator.SetTrigger("isDead");

            if (playerController != null)
                playerController.SetDeadState(true); // Freeze player movement

            if (deathScreenUI) deathScreenUI.SetActive(true);
            if (gameOverUI) gameOverUI.Show();
            if (inputManager != null) inputManager.DisablePlayerInput();
            if (weaponHandler != null) weaponHandler.enabled = false;

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSecondsRealtime(respawnDelay);

            transform.position = CheckpointManager.Instance.GetCheckpoint();

            animator.SetTrigger("Respawn");
            health.ResetHealth();

            if (playerController != null)
                playerController.SetDeadState(false);

            if (inputManager != null)
                inputManager.EnablePlayerInput();

            if (weaponHandler != null)
                weaponHandler.enabled = true;

            if (deathScreenUI) deathScreenUI.SetActive(false);
            if (gameOverUI) gameOverUI.Hide();

            isDead = false; // ✅ Allow future deaths
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnDeath -= HandleDeath;
        }
    }
}
