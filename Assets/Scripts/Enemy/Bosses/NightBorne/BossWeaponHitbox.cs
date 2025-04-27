using UnityEngine;
using Common;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Enemy.Bosses.NightBorne
{
    public class BossWeaponHitbox : MonoBehaviour
    {
        [SerializeField] private int damage = 25;
        private bool canDamage = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canDamage) return;

            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("Damaged!");
                canDamage = false;
                PauseTimeBriefly();
            }
        }

        public void EnableHitbox() => canDamage = true;
        public void DisableHitbox() => canDamage = false;

        private void PauseTimeBriefly()
        {
            Time.timeScale = 0f;
            StartCoroutine(UnfreezeTimeCoroutine());
        }

        [SuppressMessage("CodeQuality", "IDE0060:Remove unused parameter", Justification = "Required for Unity coroutine pattern.")]
        private IEnumerator UnfreezeTimeCoroutine()
        {
            yield return new WaitForSecondsRealtime(0.08f);
            Time.timeScale = 1f;
        }
    }
}
