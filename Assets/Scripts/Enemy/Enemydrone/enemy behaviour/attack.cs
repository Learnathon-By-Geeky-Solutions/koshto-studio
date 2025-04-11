using Common;
using Player;
using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    [Header("Attack Settings")]
    public int attackDamage = 100;  // Damage dealt on attack
    public float attackRange = 1.5f; // How close the enemy has to be to attack

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public void ExecuteBehavior()
    {
        if (core == null || core.player == null) return;

        // Move the enemy toward the player
        Vector2 direction = (core.player.position - core.transform.position).normalized;
        core.rb.MovePosition(core.rb.position + direction * core.moveSpeed * Time.deltaTime);

        // Check if the enemy is in attack range
        if (Vector2.Distance(core.transform.position, core.player.position) <= attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Assuming the player has a PlayerHealth script attached
        var playerHealth = core.player.GetComponent<PlayerHealth>(); // Ensure you're using PlayerHealth here

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage); // Apply the attack damage to the player
            Debug.Log("Enemy attacked the player for " + attackDamage + " damage.");
        }
    }
}
