using UnityEngine;
using Enemy;

namespace Player.Weapons
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField]
        [Tooltip("The damage this weapon deals.")]
        private int damage = 25;

        [SerializeField]
        [Tooltip("The size of the attack range box.")]
        private Vector2 attackRange = new Vector2(1f, 1f);

        [SerializeField]
        [Tooltip("Position from which the attack is cast.")]
        private Transform attackOrigin;

        [SerializeField]
        [Tooltip("Layer mask for enemies.")]
        private LayerMask enemyLayer;

        protected override void Attack()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackOrigin.position, attackRange, 0f, enemyLayer);

            foreach (Collider2D hit in hits)
            {
                Debug.Log("Melee attack triggered.");
                if (hit.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(damage);
                }
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