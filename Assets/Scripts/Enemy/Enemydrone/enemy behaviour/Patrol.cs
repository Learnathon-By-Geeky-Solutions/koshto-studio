using UnityEngine;

public class PatrolBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    [Header("Patrol Settings")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    public float patrolSpeed = 2f;

    private bool isMovingToA = true;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public void ExecuteBehavior()
    {
        if (core == null || patrolPointA == null || patrolPointB == null)
            return;

        Vector3 target = isMovingToA ? patrolPointA.position : patrolPointB.position;
        MoveTowards(target);
    }

    private void MoveTowards(Vector3 target)
    {
        float step = patrolSpeed * Time.deltaTime;
        core.rb.position = Vector2.MoveTowards(core.rb.position, target, step);

        if (Vector2.Distance(core.rb.position, target) < 0.1f)
        {
            isMovingToA = !isMovingToA;  // Switch target when the patrol point is reached
        }
    }
}
