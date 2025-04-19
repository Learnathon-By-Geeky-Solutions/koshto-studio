using UnityEngine;
using Common;

namespace Enemy
{
    [RequireComponent(typeof(EnemyCore))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Behaviors")]
        [SerializeField] private MonoBehaviour patrolBehaviorScript;
        [SerializeField] private MonoBehaviour attackBehaviorScript;
        
        private IEnemyBehavior patrolBehavior;
        private IEnemyBehavior attackBehavior;
        
        [Header("Detection")]
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask playerMask;

        private Transform player;
        private EnemyCore core;
        private Health health;
        private IEnemyBehavior currentBehavior;
        private Animator animator;

        private void Awake()
        {
            core = GetComponent<EnemyCore>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            // Cast MonoBehaviours to IEnemyBehavior
            patrolBehavior = patrolBehaviorScript as IEnemyBehavior;
            attackBehavior = attackBehaviorScript as IEnemyBehavior;

            if (patrolBehavior == null)
                Debug.LogError($"{gameObject.name}: Patrol behavior does not implement IEnemyBehavior.");

            if (attackBehavior == null)
                Debug.LogError($"{gameObject.name}: Attack behavior does not implement IEnemyBehavior.");

            health.OnDeath += HandleDeath;
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            bool isInRange = distance <= detectionRange;
            bool hasLineOfSight = false;

            if (isInRange)
            {
                Vector3 scale = transform.localScale;
                scale.x = (player.position.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                transform.localScale = scale;
                Vector2 direction = (player.position - transform.position).normalized;

                // First raycast: check if anything blocks the view
                RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, direction, detectionRange, obstacleMask);

                if (obstacleHit.collider == null)
                {
                    // No obstacle hit, now raycast to see if we hit the player
                    RaycastHit2D playerHit = Physics2D.Raycast(transform.position, direction, detectionRange, playerMask);
        
                    if (playerHit.collider != null && playerHit.collider.CompareTag("Player"))
                    {
                        hasLineOfSight = true;
                    }
                }
            }

            bool isPlayerDetected = isInRange && hasLineOfSight;

            animator.SetBool("isMoving", !isPlayerDetected); // Assume movement during patrol only

            if (isPlayerDetected)
                SetBehavior(attackBehavior);
            else
                SetBehavior(patrolBehavior);

            currentBehavior?.ExecuteBehavior(core);
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

        private static void InvokeDisableEnemy()
        {
            // Since it's now static, we can't use instance members directly.
            // You'd typically pass in a reference to the object.
            Debug.LogError("InvokeDisableEnemy called statically without GameObject reference.");
        }

        private static void DisableEnemy(GameObject enemyObject)
        {
            if (enemyObject != null)
                enemyObject.SetActive(false);
        }
    }
}
