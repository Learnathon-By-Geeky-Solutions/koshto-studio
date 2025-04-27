using UnityEngine;
using Common;
using System.Collections;

namespace Enemy.Bosses.NightBorne
{
    public class BossWeaponHitbox : MonoBehaviour
    {
        [SerializeField] private int damage = 25;
        private bool canDamage = false;
        private bool hasDamagedThisSwing = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TryDamage(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            TryDamage(collision);
        }

        private void TryDamage(Collider2D collision)
        {
            if (!canDamage || hasDamagedThisSwing) return;

            if (collision.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                hasDamagedThisSwing = true;
                Debug.Log("Boss damaged player!");
                PauseTimeBriefly();
            }
        }

        public void EnableHitbox()
        {
            canDamage = true;
            hasDamagedThisSwing = false; // Reset when starting a new attack
        }

        public void DisableHitbox()
        {
            canDamage = false;
        }

        private void PauseTimeBriefly()
        {
            Time.timeScale = 0f;
            StartCoroutine(UnfreezeTimeCoroutine());
        }

        private IEnumerator UnfreezeTimeCoroutine()
        {
            yield return new WaitForSecondsRealtime(0.08f);
            Time.timeScale = 1f;
        }
    }
}
