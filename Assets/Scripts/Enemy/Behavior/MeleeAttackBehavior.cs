using UnityEngine;
using Common;

namespace Enemy
{
    // This component should be attached to your melee enemy (like a zombie).
    // It implements the IEnemyBehavior interface and will be assigned in the Enemy component.
    public class MeleeAttackBehavior : MonoBehaviour, IEnemyBehavior
    {
        [SerializeField]
        [Tooltip("Time (in seconds) between attacks.")]
        private float attackCooldown = 1.5f;

        [SerializeField]
        [Tooltip("Damage dealt per attack.")]
        private int damage = 10;

        [SerializeField]
        [Tooltip("The maximum distance to trigger an attack.")]
        private float attackRange = 1.5f;

        private float lastAttackTime;

        // The ExecuteBehavior method now takes an EnemyCore reference.
        public void ExecuteBehavior(EnemyCore core)
        {
            // Ensure we have a valid player reference.
            if (core == null || core.Player == null) return;

            // Measure the distance between the enemy and the player.
            float distance = Vector2.Distance(core.transform.position, core.Player.position);

            // If the player is within attack range and the cooldown has elapsed...
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                // Get the Health component on the player and deal damage.
                Health playerHealth = core.Player.GetComponent<Health>();
                playerHealth?.TakeDamage(damage);

                // Trigger the attack animation.
                IEnemyAnimator animator = core.GetComponent<IEnemyAnimator>();
                animator?.PlayAttack();

            }
            else
            {
                // Optionally, if needed, reset the trigger when not in attack range.
                IEnemyAnimator animator = core.GetComponent<IEnemyAnimator>();
                animator?.PlayAttack();

            }

            // Also, you might want to have the enemy move toward the player when not attacking.
            // For instance, if the enemy is out of range, make it approach:
            if (distance > attackRange)
            {
                Vector2 direction = (core.Player.position - core.transform.position).normalized;
                // Modify only the horizontal velocity; keep vertical velocity unchanged.
                core.Rb.velocity = new Vector2(direction.x * core.MoveSpeed, core.Rb.velocity.y);
            }
        }
    }
}
