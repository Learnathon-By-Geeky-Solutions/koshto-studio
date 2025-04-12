using UnityEngine;
using Common;

namespace Enemy
{
    [RequireComponent(typeof(EnemyCore))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Behaviors")]
        [SerializeField] private PatrolBehavior patrolBehavior;
        [SerializeField] private AttackBehavior attackBehavior;

        [Header("Detection")]
        [SerializeField] private float detectionRange = 5f;

        private Transform player;
        private EnemyCore core;
        private Health health;
        private IEnemyBehavior currentBehavior;
        private Animator animator;

        private void Awake()
        {
            core = GetComponent<EnemyCore>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();

            if (patrolBehavior == null || attackBehavior == null)
                Debug.LogWarning("Enemy is missing one or more behaviors");

            health.OnDeath += HandleDeath;
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            bool isPlayerDetected = distance <= detectionRange;

            animator.SetBool("isMoving", !isPlayerDetected); // Assume movement during patrol only

            if (isPlayerDetected)
                SetBehavior(attackBehavior);
            else
                SetBehavior(patrolBehavior);

            currentBehavior?.ExecuteBehavior();
        }

        private void SetBehavior(IEnemyBehavior newBehavior)
        {
            if (currentBehavior == newBehavior) return;
            currentBehavior = newBehavior;
        }

        public void TakeDamage(int amount)
        {
            health.TakeDamage(amount);
        }

        private void HandleDeath()
        {
            animator.SetTrigger("isDead");
            Invoke(nameof(InvokeDisableEnemy), 1f);
        }

        private void InvokeDisableEnemy()
        {
            DisableEnemy(gameObject);
        }

        private static void DisableEnemy(GameObject enemyObject)
        {
            enemyObject.SetActive(false);
        }
    }
}
