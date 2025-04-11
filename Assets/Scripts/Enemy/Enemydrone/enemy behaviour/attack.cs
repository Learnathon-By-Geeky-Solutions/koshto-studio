using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    
}
