using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private IEnemyBehavior currentBehavior;

    public PatrolBehavior patrolBehavior;
    public AttackBehavior attackBehavior;

    private EnemyCore core;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    private void Update()
    {
        if (core == null || core.player == null) return;

        float distance = Vector2.Distance(transform.position, core.player.position);

        if (distance <= core.detectionRange)
        {
            SetBehavior(attackBehavior);
        }
        else
        {
            SetBehavior(patrolBehavior);
        }

        currentBehavior?.ExecuteBehavior();
    }

    private void SetBehavior(IEnemyBehavior newBehavior)
    {
        if (currentBehavior == newBehavior) return;
        currentBehavior = newBehavior;
    }
}
