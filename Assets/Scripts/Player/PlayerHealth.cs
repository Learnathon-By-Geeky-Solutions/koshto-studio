// using UnityEngine;
// using Common;
//
// namespace Player
// {
//     public class PlayerHealth : MonoBehaviour, IDamageable
//     {
//         [SerializeField] private int maxHealth = 100;
//         private int currentHealth;
//         
//         public int Current => currentHealth;
//         public int Max => maxHealth;
//
//         public event System.Action<int, int> OnHealthChanged;
//         public event System.Action OnDeath;
//
//         private void Awake()
//         {
//             currentHealth = maxHealth;
//             OnHealthChanged?.Invoke(currentHealth, maxHealth);
//         }
//
//         public void TakeDamage(int amount)
//         {
//             currentHealth -= amount;
//             currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
//
//             OnHealthChanged?.Invoke(currentHealth, maxHealth);
//
//             if (currentHealth <= 0)
//             {
//                 Die();
//             }
//         }
//
//         private void Die()
//         {
//             Debug.Log("Player died.");
//             OnDeath?.Invoke();
//             // TODO: Add death animation, restart logic etc.
//         }
//     }
// }