using UnityEngine;
using Common;

namespace Enemy
{
    public class AttackBehavior : MonoBehaviour, IEnemyBehavior
    {
        [SerializeField] private int attackDamage = 50;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float attackCooldown = 1f;

        private float lastAttackTime;
        private EnemyCore core;
        private Animator animator;

        public int AttackDamage { get => attackDamage; set => attackDamage = value; }
        public float AttackRange { get => attackRange; set => attackRange = value; }
        public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

        private void Awake()
        {
            core = GetComponent<EnemyCore>();
            animator = GetComponent<Animator>();
        }

        public void ExecuteBehavior(EnemyCore core)
        {
            if (!CanExecuteAttack(core)) return;

            lastAttackTime = Time.time;
            PerformAttack(core.Player);
        }

        private bool CanExecuteAttack(EnemyCore core)
        {
            if (core == null || core.Player == null) return false;

            float distance = Vector2.Distance(core.transform.position, core.Player.position);
            bool inRange = distance <= attackRange;
            bool offCooldown = Time.time >= lastAttackTime + attackCooldown;

            UpdateAttackAnimation(inRange);

            return inRange && offCooldown;
        }

        private void UpdateAttackAnimation(bool inRange)
        {
            if (inRange)
                animator?.SetTrigger("Attack");
            else
                animator?.ResetTrigger("Attack");
        }

        private void PerformAttack(Transform player)
        {
            var health = player.GetComponent<Health>();
            health?.TakeDamage(attackDamage);
        }
    }
}