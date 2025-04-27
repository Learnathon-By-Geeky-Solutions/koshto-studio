using UnityEngine;
using Common;

namespace Player
{
    public class PlayerHitbox : MonoBehaviour, IDamageable
    {
        private Health playerHealth;

        private void Awake()
        {
            playerHealth = GetComponentInParent<Health>();
            if (playerHealth == null)
                Debug.LogError("PlayerHitbox couldn't find Health component in parent!");
        }

        public void TakeDamage(int damage)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
