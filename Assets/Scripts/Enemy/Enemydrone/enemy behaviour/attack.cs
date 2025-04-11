using Player;
using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    [Header("Attack Settings")]
    public int attackDamage = 50;  // Damage dealt on attack
    public float attackRange = 1f; // How close the enemy has to be to attack

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public void ExecuteBehavior()
    {
        if (core == null || core.player == null) return;

        // Check if the enemy is within attack range of the player
        if (Vector2.Distance(core.transform.position, core.player.position) <= attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Assuming the player has a Health script attached
        var playerHealth = core.player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage); // Apply the attack damage
            Debug.Log("Enemy attacked the player for " + attackDamage + " damage.");
        }
    }
}
