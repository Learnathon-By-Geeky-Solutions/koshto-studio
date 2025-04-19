// Scripts/Enemy/Base/EnemyBase.cs
using UnityEngine;
using Common;

namespace Enemy
{
    [RequireComponent(typeof(EnemyCore))]
    [RequireComponent(typeof(Health))]
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        [Header("Behaviors")]
        [SerializeField] private MonoBehaviour patrolScript;
        [SerializeField] private MonoBehaviour attackScript;

        protected IEnemyBehavior patrolBehavior;
        protected IEnemyBehavior attackBehavior;

        [Header("Detection")]
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask playerMask;

        protected Transform player;
        protected EnemyCore core;
        protected Health health;
        protected Animator animator;

        protected IEnemyBehavior currentBehavior;

        protected IEnemyAnimator enemyAnimator;

        protected virtual void Awake()
        {
            core = GetComponent<EnemyCore>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            // Get animation handler
            enemyAnimator = GetComponent<IEnemyAnimator>();
            if (enemyAnimator == null)
                Debug.LogWarning($"{name}: No IEnemyAnimator attached!");

            // Existing setup...
            patrolBehavior = patrolScript as IEnemyBehavior;
            attackBehavior = attackScript as IEnemyBehavior;
            health.OnDeath += OnDeath;
        }

        protected virtual void Update()
        {
            if (player == null) return;

            bool isPlayerDetected = DetectPlayer();

            if (isPlayerDetected)
                enemyAnimator?.PlayIdle();
            else
                enemyAnimator?.PlayMove();

            enemyAnimator?.PlayMove();
            SetBehavior(isPlayerDetected ? attackBehavior : patrolBehavior);
            currentBehavior?.ExecuteBehavior(core);
        }

        protected void SetBehavior(IEnemyBehavior newBehavior)
        {
            if (currentBehavior == newBehavior) return;
            currentBehavior = newBehavior;
        }

        protected bool DetectPlayer()
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance > detectionRange) return false;

            Vector2 direction = (player.position - transform.position).normalized;

            // Flip to face the player
            FlipToward(player.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, obstacleMask | playerMask);
            return hit.collider != null && hit.collider.CompareTag("Player");
        }

        private void FlipToward(Vector3 target)
        {
            Vector3 scale = transform.localScale;
            scale.x = (target.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        public void TakeDamage(int damage) => health.TakeDamage(damage);

        protected virtual void OnDeath()
        {
            enemyAnimator?.PlayDeath(); // Trigger death anim
            Invoke(nameof(DisableSelf), 1.5f); // Delay before removing
        }

        private void DisableSelf() => gameObject.SetActive(false);
    }
}
