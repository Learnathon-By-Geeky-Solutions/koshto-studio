using UnityEngine;

namespace Enemy
{
    public class PatrolBehavior : MonoBehaviour, IEnemyBehavior
    {
        public Transform[] patrolPoints;
        public float patrolSpeed = 2f;

        private EnemyCore core;
        private int currentPointIndex = 0;

        private void Awake() => core = GetComponent<EnemyCore>();

        public void ExecuteBehavior()
        {
            if (core == null || patrolPoints.Length == 0) return;

            Transform target = patrolPoints[currentPointIndex];
            Vector2 direction = (target.position - transform.position).normalized;
            core.rb.MovePosition(core.rb.position + direction * patrolSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.5f)
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}