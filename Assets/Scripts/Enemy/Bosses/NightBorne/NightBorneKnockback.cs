using System.Collections;
using UnityEngine;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneKnockback : NightBorneCharge
    {
        [Header("Knockback")]
        [SerializeField] private Vector2 playerKnockbackForce = new Vector2(5f, 2f);
        [SerializeField] private float bossBackOffDistance = 2f;
        [SerializeField] private float reapproachDelay = 0.8f;

        private bool isBackingOff;
        private float reapproachTimer;

        public void KnockbackPlayer()
        {
            if (player.TryGetComponent<Rigidbody2D>(out var playerRb))
            {
                Vector2 force = new Vector2(facingDirection * playerKnockbackForce.x, playerKnockbackForce.y);
                playerRb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        public IEnumerator BackOffAfterAttack()
        {
            isBackingOff = true;
            reapproachTimer = reapproachDelay;

            Vector2 backOffTarget = (Vector2)transform.position +
                                  new Vector2(-facingDirection * bossBackOffDistance, 0f);

            float elapsed = 0f;
            Vector2 start = transform.position;

            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector2.Lerp(start, backOffTarget, elapsed / 0.4f);
                yield return null;
            }

            transform.position = backOffTarget;
        }

        public void HandleReapproachCooldown()
        {
            if (isBackingOff)
            {
                reapproachTimer -= Time.deltaTime;
                if (reapproachTimer <= 0f) isBackingOff = false;
            }
        }

        public bool IsBackingOff => isBackingOff;
    }
}