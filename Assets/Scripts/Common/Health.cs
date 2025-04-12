using System;
using UnityEngine;

namespace Common
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = true;
        private int currentHealth;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        private void Awake()
        {
            ResetHealth();
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(currentHealth - amount, 0);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            if (destroyOnDeath)
                Destroy(gameObject);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public int GetCurrentHealth() => currentHealth;
        public int GetMaxHealth() => maxHealth;
    }
}