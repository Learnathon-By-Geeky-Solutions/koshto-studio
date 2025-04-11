using UnityEngine;

public class PatrolBehavior : MonoBehaviour, IEnemyBehavior
{
    private EnemyCore core;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;

    private int currentPointIndex = 0;

    private void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public void ExecuteBehavior()
    {
        if (core == null || patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("PatrolBehavior: Core or patrol points missing.");
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];

        Vector2 direction = (targetPoint.position - core.transform.position).normalized;
        Vector2 newPosition = core.rb.position + direction * patrolSpeed * Time.deltaTime;

        core.rb.MovePosition(newPosition);

        float distance = Vector2.Distance(core.transform.position, targetPoint.position);
        Debug.Log($"Moving to patrol point {currentPointIndex}. Distance: {distance}");

        if (distance < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            Debug.Log("Switching to patrol point: " + currentPointIndex);
        }
    }
}
