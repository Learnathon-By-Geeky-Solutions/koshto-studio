// Enemy/Bosses/NightBorne/BossWeaponHitbox.cs
using UnityEngine;
using Common;
using UI;

namespace Enemy.Bosses.NightBorne
{
    public class BossWeaponHitbox : MonoBehaviour
    {
        [SerializeField] private int damage = 25;
        private bool canDamage = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canDamage) return;

            if (collision.CompareTag("Player") && collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);

                // Optionally: Pause time on hit
                Time.timeScale = 0f;
                Invoke(nameof(UnfreezeTime), 0.08f);
            }
        }

        private static void UnfreezeTime()
        {
            Time.timeScale = 1f;
        }

        public void EnableHitbox() => canDamage = true;
        public void DisableHitbox() => canDamage = false;
    }
}
