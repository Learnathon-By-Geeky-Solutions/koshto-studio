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
        private InputManager inputManager;
        
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
            weaponHandler = GetComponent<WeaponHandler>();

        }

        private void HandleDeath()
        {
            animator.SetTrigger("isDead");
            if (deathScreenUI) deathScreenUI.SetActive(true);
            if (gameOverUI) gameOverUI.Show();

            if (inputManager != null)
            {
                inputManager.DisablePlayerInput();
            }
            weaponHandler.enabled = false;
            StartCoroutine(Respawn());
            // equippedWeaponBeforeDeath = weaponHandler != null ? GetEquippedWeapon() : null;


        }

        // private Weapon GetEquippedWeapon()
        // {
        //     // Assuming the weapon currently active is the equipped one
        //     // You can modify this logic if your system is different
        //     return weaponHandler != null ? weaponHandler.GetComponentInChildren<Weapon>(true) : null;
        // }


        private IEnumerator Respawn()
        {
            yield return new WaitForSecondsRealtime(respawnDelay);
            weaponHandler.enabled = true;

            if (this == null || gameObject == null)
            {
                GameManager.Instance.SetupLevel();
            }
            else
            {
                // Just respawn this player at checkpoint
                transform.position = CheckpointManager.Instance.GetCheckpoint();
                if (inputManager != null)
                {
                    inputManager.EnablePlayerInput();
                }
                GetComponent<Health>().ResetHealth();
                //ensures 'you died' text doesn't appear after respawn
                if (deathScreenUI) deathScreenUI.SetActive(false);

            }
            if (equippedWeaponBeforeDeath != null && weaponHandler != null)
            {
                weaponHandler.EquipWeapon(equippedWeaponBeforeDeath);
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