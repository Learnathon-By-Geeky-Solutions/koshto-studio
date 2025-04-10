using UnityEngine;
using Player;
using Common;

namespace Enemy
{
    public class AttackBehavior : MonoBehaviour, IEnemyBehavior
    {
        public int attackDamage = 50;
        public float attackRange = 1f;
        public float attackCooldown = 1f;

        private float lastAttackTime;
        private EnemyCore core;
        private Animator animator;

        private void Awake()
        {
            core = GetComponent<EnemyCore>();
            animator = GetComponent<Animator>();
        }

        public void ExecuteBehavior()
        {
            if (core == null || core.player == null) return;

            float distance = Vector2.Distance(core.transform.position, core.player.position);

            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;

                    var health = core.player.GetComponent<Health>();
                    health?.TakeDamage(attackDamage);

                    animator?.SetTrigger("Attack");
                }
            }
            else
            {
                animator?.ResetTrigger("Attack"); // cancel attack if player moved
            }
        }
    }
}