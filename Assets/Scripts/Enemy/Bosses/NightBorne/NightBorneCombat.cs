using UnityEngine;
using System.Collections;

namespace Enemy.Bosses.NightBorne
{
    public class NightBorneCombat : NightBorneMovement
    {
        [Header("Attack")]
        [SerializeField] protected float attackRange = 1.8f;
        [SerializeField] protected float attackCooldown = 1.2f;
        [SerializeField] protected int baseDamage = 10;
        [SerializeField] protected Vector2 playerKnockbackForce = new Vector2(5f, 2f);
        [SerializeField] protected float bossBackOffDistance = 2f;

        protected bool isAttacking = false;
        protected bool inSecondPhase = false;

        protected IEnumerator AttackRoutine()
        {
            isAttacking = true;
            rb.velocity = Vector2.zero;

            animationHandler.PlayAttack();
            yield return new WaitForSeconds(0.2f);

            if (inSecondPhase)
            {
                KnockbackPlayer();
                BackOffAfterAttack();
            }

            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }

        protected void KnockbackPlayer()
        {
            if (player.TryGetComponent<Rigidbody2D>(out var playerRb))
            {
                Vector2 force = new Vector2(facingDirection * playerKnockbackForce.x, playerKnockbackForce.y);
                playerRb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        protected void BackOffAfterAttack()
        {
            if (!inSecondPhase) return;

            Vector2 backOffTarget = (Vector2)transform.position + new Vector2(-facingDirection * bossBackOffDistance, 0f);
            StartCoroutine(SmoothBackOff(backOffTarget));
        }

        protected int GetCurrentDamage()
        {
            return inSecondPhase ? Mathf.CeilToInt(baseDamage * 1.25f) : baseDamage;
        }

        public void EnableHitbox()
        {
            if (weaponHitbox != null)
            {
                int damage = GetCurrentDamage();
                weaponHitbox.SetDamage(damage);
                weaponHitbox.EnableHitbox();
            }
        }

        public void DisableHitbox()
        {
            if (weaponHitbox != null)
                weaponHitbox.DisableHitbox();
        }
    }
}