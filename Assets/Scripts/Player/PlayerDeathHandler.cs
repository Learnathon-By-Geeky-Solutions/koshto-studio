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

        private WeaponHandler weaponHandler;
        private Weapon equippedWeaponBeforeDeath;

        [SerializeField] private GameObject deathScreenUI;
        private Animator animator;
        private Health health;
        private GameOverUI gameOverUI;
        private Rigidbody2D rb;
        
        [SerializeField] private float respawnDelay = 2f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody2D>();

            health.OnDeath += HandleDeath;
            gameOverUI = FindObjectOfType<GameOverUI>();
        }

        private void HandleDeath()
        {
            animator.SetTrigger("isDead");
            if (deathScreenUI) deathScreenUI.SetActive(true);
            if (gameOverUI) gameOverUI.Show();

            GetComponent<Player.input.Player>().enabled = false;
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSecondsRealtime(respawnDelay);

            if (this == null || gameObject == null)
            {
                GameManager.Instance.SetupLevel();
            }
            else
            {
                // Just respawn this player at checkpoint
                transform.position = CheckpointManager.Instance.GetCheckpoint();
                GetComponent<Player.input.Player>().enabled = true;
                GetComponent<Health>().ResetHealth();
                //ensures 'you died' text doesn't appear after respawn
                if (deathScreenUI) deathScreenUI.SetActive(false);

            }

            GameManager.Instance.SetupLevel(); // Let GameManager spawn new player
        }
        
        
        private void OnDestroy()
        {
            if (health != null)
                health.OnDeath -= HandleDeath;
        }
    }
}