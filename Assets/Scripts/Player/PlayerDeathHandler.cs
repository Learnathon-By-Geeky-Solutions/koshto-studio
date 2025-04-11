using UnityEngine;
using Common;

namespace Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        private Animator animator;
        private Health health;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();

            health.OnDeath += HandleDeath;
        }

        private void HandleDeath()
        {
            Debug.Log("Player died");

            // Play death animation
            if (animator) animator.SetTrigger("isDead");

            // Disable player input
            var playerScript = GetComponent<Player.input.Player>();
            if (playerScript) playerScript.enabled = false;

            // Optional: Freeze time, show Game Over screen, etc.
            // Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnDeath -= HandleDeath;
        }
    }
}