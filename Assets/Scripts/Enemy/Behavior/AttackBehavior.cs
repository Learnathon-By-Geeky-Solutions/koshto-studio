using UnityEngine;
using Common;

namespace Enemy
{
    public class AttackBehavior : MonoBehaviour, IEnemyBehavior
    {
        [SerializeField]
        private int attackDamage = 50;
        public int AttackDamage
        {
            get => attackDamage;
            set => attackDamage = value;
        }

        [SerializeField]
        private float attackRange = 1f;
        public float AttackRange
        {
            get => attackRange;
            set => attackRange = value;
        }

        [SerializeField]
        private float attackCooldown = 1f;
        public float AttackCooldown
        {
            get => attackCooldown;
            set => attackCooldown = value;
        }

        private float lastAttackTime;
        private EnemyCore core;
        private Animator animator;

        private void Awake()
        {
            core = GetComponent<EnemyCore>();
            animator = GetComponent<Animator>();
        }

        public void ExecuteBehavior(EnemyCore core)
        {
            if (core == null || core.Player == null) return;

            float distance = Vector2.Distance(core.transform.position, core.Player.position);

            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;

                    var health = core.Player.GetComponent<Health>();
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