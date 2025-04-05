using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Max health of the enemy.")]
        private int maxHealth = 100;

        private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Apply damage to the enemy.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0);

            Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            Debug.Log($"{gameObject.name} died.");
            // You could add animation, particles, sounds, etc.
            Destroy(gameObject);
        }
    }
}