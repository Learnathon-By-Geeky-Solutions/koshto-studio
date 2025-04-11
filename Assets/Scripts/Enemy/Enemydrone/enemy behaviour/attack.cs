using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public void ExecuteBehavior()
    {
        if (core == null || core.player == null) return;

        Vector2 direction = (core.player.position - core.transform.position).normalized;
        core.rb.MovePosition(core.rb.position + direction * core.moveSpeed * Time.deltaTime);

        // Add your attack animation or bullet firing here
        Debug.Log("Attacking Player!");
    }
}
