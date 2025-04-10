using UnityEngine;
using Common;

namespace Player.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField, Tooltip("The damage this weapon deals.")]
        private int damage = 25;

        [SerializeField, Tooltip("The size of the attack range box.")]
        private Vector2 attackRange = new Vector2(1f, 1f);

        [SerializeField, Tooltip("Position from which the attack is cast.")]
        private Transform attackOrigin;

        [SerializeField, Tooltip("Layer mask for enemies.")]
        private LayerMask enemyLayer;

        protected override void Attack()
        {
            if (!IsConfigured()) return;

            var hits = Physics2D.OverlapBoxAll(attackOrigin.position, attackRange, 0f, enemyLayer);

            foreach (var hit in hits)
            {
                TryDamageEnemy(hit);
            }

            Debug.Log("Melee attack executed.");
        }

        private bool IsConfigured()
        {
            if (attackOrigin == null)
            {
                Debug.LogWarning("Attack origin not set.");
                return false;
            }

            if (attackRange.x <= 0 || attackRange.y <= 0)
            {
                Debug.LogWarning("Invalid attack range.");
                return false;
            }

            return true;
        }

        private void TryDamageEnemy(Collider2D collider)
        {
            if (collider.TryGetComponent(out Common.IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (attackOrigin == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackOrigin.position, attackRange);
        }
#endif
    }
}