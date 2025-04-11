using UnityEngine;

public class PatrolBehavior : MonoBehaviour, IEnemyBehavior
{
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;
    private Vector2 currentTarget;
    private EnemyCore core;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
        currentTarget = patrolPointA;
    }

    public void ExecuteBehavior()
    {
        if (core == null) return;

        core.rb.MovePosition(Vector2.MoveTowards(core.rb.position, currentTarget, core.moveSpeed * Time.deltaTime));

        if (Vector2.Distance(core.rb.position, currentTarget) < 0.1f)
        {
            currentTarget = currentTarget == patrolPointA ? patrolPointB : patrolPointA;
        }
    }
}
