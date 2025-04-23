using UnityEngine;

namespace Enemy
{
    public class PatrolBehavior : MonoBehaviour, IEnemyBehavior
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float patrolSpeed = 2f;

        private int currentPointIndex = 0;

        public Transform[] PatrolPoints
        {
            get => patrolPoints;
            set => patrolPoints = value;
        }

        public float PatrolSpeed
        {
            get => patrolSpeed;
            set => patrolSpeed = value;
        }

        public void ExecuteBehavior(EnemyCore core)
        {
            if (core == null || patrolPoints == null || patrolPoints.Length == 0) return;

            Transform target = patrolPoints[currentPointIndex];
            Vector2 direction = (target.position - core.transform.position).normalized;
            core.Rb.MovePosition(core.Rb.position + direction * patrolSpeed * Time.deltaTime);

            if (Vector2.Distance(core.transform.position, target.position) < 0.5f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }
}
