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

    
}
